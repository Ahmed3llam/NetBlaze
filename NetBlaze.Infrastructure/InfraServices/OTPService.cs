using NetBlaze.Application.Interfaces.General;

namespace NetBlaze.Infrastructure.InfraServices
{
    public class OTPService : IOTPService
    {
        public string GenerateOtpAsync(CancellationToken cancellationToken = default)
        {
            var random = new Random();
            string otp = random.Next(100000, 999999).ToString();
            return otp;
        }
    }
}
