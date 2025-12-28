using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.User.Request;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.Ui.Client.Services;
using System.Text.RegularExpressions;

namespace NetBlaze.Ui.Client.Pages
{
    public partial class LoginPage
    {
        [Inject] BlazAuthService BlazAuthService { get; set; } = null!;

        private LoginUserRequestDto loginUserRequestDto = new();
        private bool IsSubmitButtonDisabled =>
          string.IsNullOrWhiteSpace(loginUserRequestDto.Email) ||
          string.IsNullOrWhiteSpace(loginUserRequestDto.Password) ||
          !Regex.IsMatch(loginUserRequestDto.Email, RegexTemplate.Email) ||
          !Regex.IsMatch(loginUserRequestDto.Password, RegexTemplate.Password);

        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private bool Showed = false;
        InputType PasswordInput = InputType.Password;

        private async Task Login()
        {
            var response = await BlazAuthService.Login(loginUserRequestDto);
            if(response.Success)
            {
                var bearerToken = response.Data.Token.Replace("\"", "");
                await BlazAuthService.SaveEncryptedTokenToLocalStorageAsync(bearerToken);
                NavigationManager.NavigateTo("/");
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
    }
}
