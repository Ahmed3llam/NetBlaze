using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.SharedResources;

namespace NetBlaze.Ui.Client.Components.Generic
{
    public partial class GenericLoadingButton
    {
        [Parameter] public EventCallback OnClick { get; set; }
        [Parameter] public string ButtonText { get; set; } = StaticLocalization.Submit;
        [Parameter] public string LoadingText { get; set; } = StaticLocalization.InitializingPleaseWait;
        [Parameter] public bool Disabled { get; set; } = false;
        [Parameter] public Size Size { get; set; } = Size.Medium;
        [Parameter] public bool FullWidth { get; set; }
        [Parameter] public string Class { get; set; } = "m-2";
        [Parameter] public ButtonType Type { get; set; } = ButtonType.Button;
        [Parameter] public Color ButtonColor { get; set; } = Color.Primary;
        [Parameter] public string? StartIcon { get; set; }
        [Parameter] public string? EndIcon { get; set; }
        [Parameter] public Variant Variant { get; set; } = Variant.Filled;

        private bool IsLoading { get; set; } = false;
        private bool ComputedDisabled => Disabled || IsLoading;

        private async Task HandleClick()
        {
            if (IsLoading) return;

            IsLoading = true;

            StateHasChanged();

            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync();
            }

            IsLoading = false;

            StateHasChanged();
        }
    }
}
