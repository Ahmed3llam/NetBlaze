using Microsoft.AspNetCore.Components;
using MudBlazor;
using NetBlaze.SharedKernel.Dtos.General;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.Ui.Client.Components.Generic;
using NetBlaze.Ui.Client.Dialogs.Vacation;
using NetBlaze.Ui.Client.InternalHelperTypes.General;
using NetBlaze.Ui.Client.Services;

namespace NetBlaze.Ui.Client.Components.Concrete.Vacation
{
    public partial class VacationCrudOperationsComponent
    {
        [Inject] IDialogService DialogService { get; set; } = null!;

        [Inject] BlazVacationService BlazVacationService { get; set; } = null!;

        private PaginatedList<GetListedVacationResponseDto> _vacations;

        private CancellationTokenSource cts = new();

        private bool _isLoading = false;

        private int _pageNumber = 1;

        private int _pageSize = 9;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadVacationsAsync();

                StateHasChanged();
            }
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    await LoadVacationsAsync();
        //}

        private async Task LoadVacationsAsync()
        {
            _isLoading = true;
            StateHasChanged();

            var response = await BlazVacationService.GetListedVacationsAsync(new PaginateRequestDto
            {
                PageNumber = _pageNumber,
                PageSize = _pageSize
            });

            if (response.Success && response.Data is not null)
            {
                _vacations = response.Data;
            }

            _isLoading = false;
            StateHasChanged();
        }

        private async Task OpenAddDialogAsync()
        {
            var parameters = new DialogParameters();

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<AddVacationDialog>("Add New Vacation", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadVacationsAsync();
            }
        }

        private async Task OpenUpdateDialogAsync(GetListedVacationResponseDto vacation)
        {
            var fullVacationResponse = await BlazVacationService.GetVacationByIdAsync(vacation.Id);

            if (!fullVacationResponse.Success)
            {
                return;
            }

            var parameters = new DialogParameters<UpdateVacationDialog>
            {
                { x => x.GetVacationResponseDto, fullVacationResponse.Data }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Large,
                FullWidth = true
            };

            var dialog = await DialogService.ShowAsync<UpdateVacationDialog>("Update Existing Vacation", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadVacationsAsync();
            }
        }

        private async Task DeleteSampleAsync(long id)
        {
            var parameters = new DialogParameters<GenericDialog>
            {
                { x => x.Title, "Delete Existing Vacation"},
                { x => x.Content, "Are you sure you want to delete this vacation? This action cannot be undone." },
                { x => x.CancelText, "Cancel" },
                { x => x.SubmitText, "Delete" }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true
            };
            var dialog = await DialogService.ShowAsync<GenericDialog>(string.Empty, parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                _isLoading = true;
                StateHasChanged();

                var response = await BlazVacationService.DeleteVacationAsync(id);

                if (response.Success)
                {
                    await LoadVacationsAsync();
                }

                _isLoading = false;
                StateHasChanged();
            }
        }
    }
}
