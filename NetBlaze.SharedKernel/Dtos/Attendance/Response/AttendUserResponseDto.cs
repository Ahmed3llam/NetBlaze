namespace NetBlaze.SharedKernel.Dtos.Attendance.Response
{
    public sealed record AttendUserResponseDto(
       long Id,
       long UserId,
       DateOnly Date,
       TimeSpan Time
   );
}
