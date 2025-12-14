
namespace NetBlaze.SharedKernel.Dtos.General
{
    public record PaginateRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 9;
    }
}
