using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Vacation
{
    public partial class AddVacationDialog
    {
        [Inject] BlazVacationService BlazVacationService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        private AddVacationRequestDto _model = new();

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazVacationService.AddVacationAsync(_model);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_model));
            }
        }
    }
}
