using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Vacation.Responses
{
    public sealed record GetListedVacationResponseDto(
        long Id,
        VacationType VacationType,
        DayOfWeek? Day,
        DateTime? VacationDate,
        DateTime? AlternativeDate,
        string? Description
    );
}
