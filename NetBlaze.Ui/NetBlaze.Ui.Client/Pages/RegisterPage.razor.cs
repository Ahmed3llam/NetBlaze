using Microsoft.AspNetCore.Components;
using NetBlaze.SharedKernel.Dtos.Sample.Responses;
using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class RegisterPage
    {
        [Inject] BlazAuthService BlazAuthService { get; set; } = null!;
        [Inject] BlazDepartmentService blazDepartmentService { get; set; } = null!;
        [Inject] BlazRolesService blazRolesService { get; set; } = null!;

        private RegisterUserRequestDto registerUserRequestDto = new();

        private CancellationTokenSource cts = new();

        private List<BaseResponseDto> _departments = [];

        private List<BaseResponseDto> _roles = [];

        protected override async Task OnInitializedAsync()
        {
            await LoadDepartmentsAsync();
            await LoadRolesAsync();
        }

        private async Task LoadDepartmentsAsync()
        {
            StateHasChanged();

            _departments.Clear();

            await foreach (var department in blazDepartmentService.GetListedDepartments(cts.Token))
            {
                _departments.Add(department);

                await InvokeAsync(StateHasChanged);
            }

            StateHasChanged();
        }

        private async Task LoadRolesAsync()
        {
            StateHasChanged();

            _roles.Clear();

            await foreach (var role in blazRolesService.GetListedRoles(cts.Token))
            {
                _roles.Add(role);

                await InvokeAsync(StateHasChanged);
            }

            StateHasChanged();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazAuthService.Register(registerUserRequestDto);

            if (response != null && response.Success)
            {
                RegisterFidoUserRequestDto registerFidoUserRequestDto = new RegisterFidoUserRequestDto()
                {
                    UserId = response.Data.Id,
                };

                var options = await BlazAuthService.RegisterFidoStart(registerFidoUserRequestDto, cts.Token);
            }
        }
    }
}
