using NetBlaze.SharedKernel.Enums;

namespace NetBlaze.SharedKernel.Dtos.Attendance.Response
{
    public sealed record GetEmployeeLatenessResponseDto(
       long EmployeeId,
       string EmployeeName,
       long DepartmentId,
       string DepartmentName,
       long AttendanceId,
       DateOnly Date,
       TimeSpan CheckInTime,
       long PolicyId,
       string PolicyName,
       PolicyAction PolicyAction,
       double ActionValue,
       int LateMinutes,
       bool IsActionTaken,
       double? AppliedHours,
       bool? ActionAppliedStatus,
       string? ActionClarification
    );
}
