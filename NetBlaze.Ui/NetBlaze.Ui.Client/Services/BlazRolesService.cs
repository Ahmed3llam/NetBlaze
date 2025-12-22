using NetBlaze.SharedKernel.Dtos.Shared.Responses;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.Ui.Client.Services.CommonServices;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace NetBlaze.Ui.Client.Services
{
    public class BlazRolesService : BaseBlazService
    {
        public BlazRolesService(ExternalHttpClientWrapper externalHttpClientWrapper, CentralizedSnackbarProvider centralizedSnackbarProvider) : base(externalHttpClientWrapper, centralizedSnackbarProvider) { }

        public async IAsyncEnumerable<BaseResponseDto> GetListedRoles([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var response = await _externalHttpClientWrapper.NativeHttpClient.GetAsync(ApiRelativePaths.ROLE_LIST, cancellationToken);

            response.EnsureSuccessStatusCode();

            await foreach (var role in response.Content.ReadFromJsonAsAsyncEnumerable<BaseResponseDto>(cancellationToken))
            {
                if (role != null)
                {
                    yield return role;
                }
            }
        }
    }
}
