using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class Register
    {
        [Inject] BlazAuthService BlazAuthService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        private RegisterUserRequestDto registerUserRequestDto = new();

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            registerUserRequestDto.DepartmentId = 1;
            var response = await BlazAuthService.Register(registerUserRequestDto);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(registerUserRequestDto));
            }
        }
    }
}
