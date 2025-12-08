
namespace NetBlaze.Application.Interfaces.General
{
    public interface IUserContext
    {
        public string Token { get; }
        public long UserId { get; }
    }
}
