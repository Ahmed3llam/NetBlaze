using Microsoft.AspNetCore.Http;
using NetBlaze.Application.Interfaces.General;

namespace NetBlaze.Infrastructure.InfraServices
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token {
            get
            {
                var authHeader = _httpContextAccessor.HttpContext?
                    .Request
                    .Headers["Authorization"]
                    .ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                    return null;

                return authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authHeader.Substring("Bearer ".Length)
                    : authHeader;
            }
        }
        public long UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
