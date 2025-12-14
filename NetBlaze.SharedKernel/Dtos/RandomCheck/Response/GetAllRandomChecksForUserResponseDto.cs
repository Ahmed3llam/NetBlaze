namespace NetBlaze.SharedKernel.Dtos.RandomCheck.Response
{
    public sealed record GetAllRandomChecksForUserResponseDto(
        long EmployeeId,
        string EmployeeName,
        long DepartmentId,
        string DepartmentName,
        DateOnly Date,
        TimeSpan Time,
        bool IsReplied = false,
        DateTime? RepliedDate = null
    );
}
