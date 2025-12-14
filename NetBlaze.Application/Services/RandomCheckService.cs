using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetBlaze.Application.Interfaces.General;
using NetBlaze.Application.Interfaces.ServicesInterfaces;
using NetBlaze.Application.Mappings;
using NetBlaze.Domain.Entities;
using NetBlaze.Domain.Entities.Identity;
using NetBlaze.Domain.Views;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Requests;
using NetBlaze.SharedKernel.Dtos.RandomCheck.Response;
using NetBlaze.SharedKernel.HelperUtilities.Constants;
using NetBlaze.SharedKernel.HelperUtilities.General;
using NetBlaze.SharedKernel.SharedResources;
using System.Net;

namespace NetBlaze.Application.Services
{
    public class RandomCheckService : IRandomCheckService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public RandomCheckService(
            IUnitOfWork unitOfWork,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<ApiResponse<object>> GenerateOtpAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;

            int expiryMinutes = _config.GetValue<int>(MiscConstants.RandomCheckExpiryInMinutes);

            var users = await _unitOfWork.Repository.GetMultipleAsync<User>(
                true,
                cancellationToken);

            List<RandomCheck> randomChecks = new List<RandomCheck>();

            foreach (var user in users)
            {
                var random = new Random();
                string otp = random.Next(100000, 999999).ToString();

                var randomCheck = new RandomCheck
                {
                    UserId = user.Id,
                    OTP = otp,
                    Date = DateOnly.FromDateTime(now.Date),
                    Time = now.TimeOfDay,
                    ExpirationDate = now.AddMinutes(expiryMinutes)
                };

                randomChecks.Add(randomCheck);
            }

            await _unitOfWork.Repository.AddRangeAsync(randomChecks, cancellationToken);

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);

            return ApiResponse<object>.ReturnSuccessResponse(null, Messages.RandomCheckCreatedSuccessfully);
        }

        public async Task<ApiResponse<bool>> RandomCheckReplyAsync(RandomCheckRequestReplyDto randomCheckRequestReply, CancellationToken cancellationToken = default)
        {
            var targetRandom = await _unitOfWork
                .Repository
                .GetQueryable<RandomCheck>()
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync(x => x.UserId == randomCheckRequestReply.UserId);

            if (targetRandom == null)
            {
                return ApiResponse<bool>.ReturnFailureResponse(Messages.RandomCheckNotFound, HttpStatusCode.NotFound);
            }

            if (targetRandom.OTP != randomCheckRequestReply.OTP)
            {
                return ApiResponse<bool>.ReturnFailureResponse(Messages.InCorrectOTP, HttpStatusCode.NotFound);
            }

            if (targetRandom.ExpirationDate < DateTime.Now)
            {
                return ApiResponse<bool>.ReturnFailureResponse(Messages.OtpExpired, HttpStatusCode.NotFound);
            }

            targetRandom.IsReplied = true;
            targetRandom.RepliedDate = DateTime.Now;

            await _unitOfWork.Repository.CompleteAsync(cancellationToken);
            
            return ApiResponse<bool>.ReturnSuccessResponse(false, Messages.RecordedSuccessfully);
        }

        public async Task<ApiResponse<PaginatedList<GetAllRandomChecksForUserResponseDto>>> GetAllRandomChecksForUser(GetAllRandomChecksForUserRequestDto getAllRandomChecksForUserRequestDto, CancellationToken cancellationToken = default)
        {
            var data = _unitOfWork.Repository
               .GetQueryable<RandomCheckReport>()
               .Where(a => 
                    a.EmployeeId == getAllRandomChecksForUserRequestDto.UserId &&
                    a.Date >= getAllRandomChecksForUserRequestDto.From && 
                    a.Date <= getAllRandomChecksForUserRequestDto.To)
               .Select(a => new GetAllRandomChecksForUserResponseDto(
                   a.EmployeeId,
                   a.EmployeeName,
                   a.DepartmentId,
                   a.DepartmentName,
                   a.Date,
                   a.Time,
                   a.IsReplied,
                   a.RepliedDate
               ));

            var response = await data.PaginatedListAsync(getAllRandomChecksForUserRequestDto.PageNumber, getAllRandomChecksForUserRequestDto.PageSize);

            return ApiResponse<PaginatedList<GetAllRandomChecksForUserResponseDto>>.ReturnSuccessResponse(response);
        }
    }
}
