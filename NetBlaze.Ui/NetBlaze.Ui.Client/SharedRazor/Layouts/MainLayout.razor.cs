using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.Ui.Client.InternalHelperTypes.Constants;
using NetBlaze.Ui.Client.Services;
using System.Globalization;

namespace NetBlaze.Ui.Client.SharedRazor.Layouts
{
    public partial class MainLayout
    {
        [Inject] public BlazAuthService BlazAuthService { get; set; } = default!;
        [Inject] public GlobalUserContext GlobalUserContext { get; set; } = default!;

        private MudTheme NetBlazeTheme { get; init; } = SharedTheme.NetBlazeTheme;

        private bool _open = false;

        private readonly bool _rightToLeft = CultureInfo.CurrentCulture.Name == LanguageCode.ARABIC_CODE;

        private bool _isGlobalUserContextFilled;

        //protected override async Task OnInitializedAsync()
        //{
        //    await BlazAuthService.HandleAuthenticationProcessForUserAsync();

        //    _isGlobalUserContextFilled = true;
        //}

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await BlazAuthService.HandleAuthenticationProcessForUserAsync();

        //        StateHasChanged();
        //    }
        //}

        private void ToggleDrawer()
        {
            _open = !_open;
        }
    }
}
