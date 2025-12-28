using Fido2NetLib;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.SharedResources;
using NetBlaze.Ui.Client.Services;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class RegisterPage
    {
        [Inject] BlazAuthService BlazAuthService { get; set; } = null!;
        [Inject] BlazUserService blazUserService { get; set; } = null!;
        [Inject] BlazDepartmentService blazDepartmentService { get; set; } = null!;
        [Inject] BlazRolesService blazRolesService { get; set; } = null!;
        [Inject] NavigationManager Nav { get; set; } = null!;
        [Inject] IJSRuntime JS { get; set; } = null!;
        [Inject] CentralizedSnackbarProvider Snackbar { get; set; } = null!;

        private RegisterUserRequestDto _model = new();

        private CancellationTokenSource cts = new();

        private List<BaseResponseDto> _managers = [];

        private List<BaseResponseDto> _departments = [];

        private List<BaseResponseDto> _roles = [];

        private CredentialCreateOptions? _fidoOptions;
        private bool IsSubmitButtonDisabled =>
          string.IsNullOrWhiteSpace(_model.FullName) ||
          string.IsNullOrWhiteSpace(_model.PhoneNumber) ||
          string.IsNullOrWhiteSpace(_model.Email) ||
          string.IsNullOrWhiteSpace(_model.ConfirmPassword) ||
          string.IsNullOrWhiteSpace(_model.Password) ||
          !Regex.IsMatch(_model.PhoneNumber, RegexTemplate.Phone) ||
          !Regex.IsMatch(_model.Email, RegexTemplate.Email) ||
          !Regex.IsMatch(_model.Password, RegexTemplate.Password) ||
          _model.DepartmentId == 0 ||
          _model.RoleId == 0;

        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool Showed = false;

        InputType PasswordInput = InputType.Password;

        protected override async Task OnInitializedAsync()
        {
            await LoadDepartmentsAsync();
            await LoadRolesAsync();
            await LoadManagersAsync();
        }

        private async Task LoadManagersAsync()
        {
            StateHasChanged();

            _managers.Clear();

            var result = await blazUserService.GetListedManagersAsync(cts.Token);

            if (result != null && result.Success && result.Data != null)
            {
                _managers.AddRange(result.Data);
            }

            StateHasChanged();
        }

        private async Task LoadDepartmentsAsync()
        {
            StateHasChanged();

            _departments.Clear();

            var result = await blazDepartmentService.GetListedDepartments(cts.Token);

            if (result != null && result.Success && result.Data != null)
            {
                _departments.AddRange(result.Data);
            }

            StateHasChanged();
        }

        private async Task LoadRolesAsync()
        {
            StateHasChanged();

            _roles.Clear();

            var result = await blazRolesService.GetListedRoles(cts.Token);

            if (result != null && result.Success && result.Data != null)
            {
                _roles.AddRange(result.Data);
            }

            StateHasChanged();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazAuthService.Register(_model);

            if (response != null && response.Success)
            {
                RegisterFidoUserRequestDto registerFidoUserRequestDto = new RegisterFidoUserRequestDto()
                {
                    UserId = response.Data.Id,
                };

                var options = await BlazAuthService.RegisterFidoStart(registerFidoUserRequestDto, cts.Token);

                if (options != null && options.Success)
                {
                    _fidoOptions = options.Data;
                    await TriggerWebAuthn();
                    StateHasChanged();
                }
            }
        }

        void TogglePasswordVisibility()
        {
            if (Showed)
            {
                Showed = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                Showed = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private async Task TriggerWebAuthn()
        {
            if (_fidoOptions == null)
            {
                Snackbar.ShowNormalSnackbar(StaticLocalization.FidoOptionsNotAvailable, Severity.Error);
                StateHasChanged();
                return;
            }

            var dotNetRef = DotNetObjectReference.Create(this);
            await JS.InvokeVoidAsync(JavaScriptMethodName.StartWebAuthnRegistration, _fidoOptions, dotNetRef);
        }

        [JSInvokable]
        public async Task OnFidoRegistrationComplete(object responseObj)
        {
            StateHasChanged();

            if (responseObj is JsonElement json && json.TryGetProperty("error", out var err))
            {
                Snackbar.ShowNormalSnackbar(StaticLocalization.FidoRegistrationFailed, Severity.Error);
                StateHasChanged();
                return;
            }

            var response = JsonSerializer.Deserialize<AuthenticatorAttestationRawResponse>(
                responseObj.ToString()!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (response == null)
            {
                Snackbar.ShowNormalSnackbar(StaticLocalization.FidoDataReadFailed, Severity.Error);
                StateHasChanged();
                return;
            }

            var result = await BlazAuthService.RegisterFidoComplete(response);

            if (result.Success)
            {
                Nav.NavigateTo(nameof(LoginPage), true);
            }
        }
    }
}
