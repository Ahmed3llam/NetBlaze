
namespace NetBlaze.SharedKernel.Dtos.Shared.Responses
{
    public sealed record BaseResponseDto(
        long Id,
        string Name,
        bool IsActive
    );
}
