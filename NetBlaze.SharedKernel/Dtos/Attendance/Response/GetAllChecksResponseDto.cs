namespace NetBlaze.SharedKernel.Dtos.Attendance.Response
{
    public sealed record GetAllChecksResponseDto(
        long EmployeeId,
        string EmployeeName,
        long DepartmentId,
        string DepartmentName,
        DateOnly Date,
        TimeSpan? CheckIn,
        TimeSpan? CheckOut,
        decimal HoursWorked
    );
}
