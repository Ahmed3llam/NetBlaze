using Microsoft.AspNetCore.Components;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class LoginPage
    {
        [Inject] BlazAuthService BlazAuthService { get; set; } = null!;

        private LoginUserRequestDto loginUserRequestDto = new();

        private async Task Login()
        {
            var response = await BlazAuthService.Login(loginUserRequestDto);
        }
    }
}
