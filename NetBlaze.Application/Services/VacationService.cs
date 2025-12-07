using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.Vacation.Requests;
using NetBlaze.SharedKernel.Dtos.Vacation.Responses;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class VacationService : IVacationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VacationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async IAsyncEnumerable<GetListedVacationResponseDto> GetListedVacations()
        {
            var listedVacations = _unitOfWork
                .Repository
                .GetMultipleStream<Vacation, GetListedVacationResponseDto>(
                    true,
                    _ => true,
                    v => new GetListedVacationResponseDto(
                        v.Id,
                        v.VacationType,
                        v.Day,
                        v.VacationDate,
                        v.AlternativeDate,
                        v.Description)
                );

            await foreach (var vacation in listedVacations)
            {
                yield return vacation;
            }
        }

        public async Task<ApiResponse<GetVacationResponseDto>> GetVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            var vacationDto = await _unitOfWork
                .Repository
                .GetSingleAsync<Vacation, GetVacationResponseDto>(
                    true,
                    v => v.Id == id,
                    v => new GetVacationResponseDto(
                        v.Id,
                        v.VacationType,
                        v.Day,
                        v.VacationDate,
                        v.AlternativeDate,
                        v.Description),
                    cancellationToken
                );

            if (vacationDto != null)
            {
                return ApiResponse<GetVacationResponseDto>.ReturnSuccessResponse(vacationDto);
            }

            return ApiResponse<GetVacationResponseDto>.ReturnFailureResponse(Messages.VacationNotFound, HttpStatusCode.NotFound);
        }

        public async Task<ApiResponse<object>> AddVacationAsync(AddVacationRequestDto addVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var vacation = Vacation.Create(addVacationRequestDto);

            await _unitOfWork.Repository.AddAsync(vacation, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(vacation.Id, Messages.VacationAddedSuccessfully);
        }

        public async Task<ApiResponse<object>> UpdateVacationAsync(UpdateVacationRequestDto updateVacationRequestDto, CancellationToken cancellationToken = default)
        {
            var targetVacation = await _unitOfWork
                .Repository
                .GetSingleAsync<Vacation>(
                    false,
                    v => v.Id == updateVacationRequestDto.Id,
                    cancellationToken
                );

            if (targetVacation == null)
                return ApiResponse<object>.ReturnFailureResponse(Messages.VacationNotFound, HttpStatusCode.NotFound);

            targetVacation.Update(updateVacationRequestDto);

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<object>.ReturnSuccessResponse(null, Messages.VacationUpdatedSuccessfully);
            }

            return ApiResponse<object>.ReturnSuccessResponse(null, Messages.VacationNotModified, HttpStatusCode.NotModified);
        }

        public async Task<ApiResponse<object>> DeleteVacationAsync(long id, CancellationToken cancellationToken = default)
        {
            var targetVacation = await _unitOfWork
                .Repository
                .GetSingleAsync<Vacation>(
                    false,
                    v => v.Id == id,
                    cancellationToken
                );

            if (targetVacation == null)
            {
                return ApiResponse<object>.ReturnFailureResponse(Messages.VacationNotFound, HttpStatusCode.NotFound);
            }

            targetVacation.SetIsDeletedToTrue();

            var rowsAffected = await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            if (rowsAffected > 0)
            {
                return ApiResponse<object>.ReturnSuccessResponse(null, Messages.VacationDeletedSuccessfully);
            }

            return ApiResponse<object>.ReturnSuccessResponse(null, Messages.VacationNotModified, HttpStatusCode.NotModified);
        }
    }
}
