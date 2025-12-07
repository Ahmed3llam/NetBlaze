using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class UserDetails
    {
        // Properties
        public long Id { get; set; }

        public Guid Key { get; set; }

        public string DeviceNumber { get; set; }

        public string CertificatePassword { get; set; }

        public long UserId { get; set; }


        // Navigational Properties

        public virtual User User { get; set; }
    }
}
