namespace NetBlaze.SharedKernel.Dtos.User.Response
{
    public sealed record LoginUserResponseDto(
        long Id,
        string FullName,
        string UserName,
        string Token,
        string Email,
        string Phone
    );
}
