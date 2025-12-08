
namespace NetBlaze.Application.Interfaces.General
{
    public interface IOTPService
    {
        public string GenerateOtpAsync(CancellationToken cancellationToken = default);
    }
}
