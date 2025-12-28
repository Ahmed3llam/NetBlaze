using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Dialogs.Vacation
{
    public partial class UpdateVacationDialog
    {
        [Inject] BlazVacationService BlazVacationService { get; set; } = null!;

        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public GetVacationResponseDto GetVacationResponseDto { get; set; } = null!;


        private UpdateVacationRequestDto _model = new();

        protected override void OnInitialized()
        {
            if (GetVacationResponseDto != null)
            {
                _model = new UpdateVacationRequestDto(
                    GetVacationResponseDto.Id,
                    GetVacationResponseDto.VacationType,
                    GetVacationResponseDto.Day,
                    GetVacationResponseDto.VacationDate,
                    GetVacationResponseDto.AlternativeDate,
                    GetVacationResponseDto.Description
                );
            }
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await BlazVacationService.UpdateVacationAsync(_model);

            if (response.Success)
            {
                MudDialog.Close(DialogResult.Ok(_model));
            }
        }
    }
}
