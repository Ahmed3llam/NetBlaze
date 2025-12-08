using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.SharedKernel.Dtos.General;
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

        public async Task<ApiResponse<PaginatedList<GetListedVacationResponseDto>>> GetListedVacations(PaginateRequestDto paginateRequestDto)
        {
            var listedVacations = _unitOfWork
                .Repository
                .GetQueryable<Vacation>()
                .Select(v => new GetListedVacationResponseDto(
                        v.Id,
                        v.VacationType,
                        v.Day,
                        v.VacationDate,
                        v.AlternativeDate,
                        v.Description)
                );

            var result = await listedVacations.PaginatedListAsync(paginateRequestDto.PageNumber, paginateRequestDto.PageNumber);

            //var result = await PaginatedList<GetListedVacationResponseDto>.CreateAsync(listedVacations, paginateRequestDto.PageNumber, paginateRequestDto.PageSize);

            return ApiResponse<PaginatedList<GetListedVacationResponseDto>>.ReturnSuccessResponse(result);
        }

        public async Task<ApiResponse<GetVacationResponseDto>> GetVacationByIdAsync(long id, CancellationToken cancellationToken = default)
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
            var vacation = new Vacation
            {
                VacationType = addVacationRequestDto.VacationType,
                Day = addVacationRequestDto.Day,
                VacationDate = addVacationRequestDto.VacationDate,
                AlternativeDate = addVacationRequestDto.AlternativeDate,
                Description = addVacationRequestDto.Description
            };

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

            //ToDo: update only sent data
            targetVacation.VacationType = updateVacationRequestDto.VacationType;
            targetVacation.Day = updateVacationRequestDto.Day;
            targetVacation.VacationDate = updateVacationRequestDto.VacationDate;
            targetVacation.AlternativeDate = updateVacationRequestDto.AlternativeDate;
            targetVacation.Description = updateVacationRequestDto.Description;

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

            targetVacation.ToggleIsActive();
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
