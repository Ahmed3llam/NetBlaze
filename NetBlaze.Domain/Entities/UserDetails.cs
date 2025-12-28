using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class UserCredential : BaseEntity<long>
    {
        // Properties

        public byte[] CredentialId { get; set; } = null!;

        public byte[] PublicKey { get; set; } = null!;

        public uint SignatureCounter { get; set; }

        public string CredType { get; set; } = null!;

        public Guid AaGuid { get; set; }

        public string? DeviceInfo { get; set; }

        public string? UserAgent { get; set; }

        public string? IpAddress { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? RevokedBy { get; set; }

        public string? RevokeReason { get; set; }

        public long UserId { get; set; }


        // Navigational Properties

        public virtual User User { get; set; } = null!;
    }
}
