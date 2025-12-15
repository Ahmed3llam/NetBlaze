using NetBlaze.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetBlaze.Domain.Entities
{
    public class UserDetails
    {
        //// Properties
        //public long Id { get; set; }

        //public Guid Key { get; set; }

        //public string DeviceNumber { get; set; }

        //public string CertificatePassword { get; set; }

        //public long UserId { get; set; }


        //// Navigational Properties

        //public virtual User User { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        public byte[] CredentialId { get; set; } = null!;

        [Required]
        public byte[] PublicKey { get; set; } = null!;

        [Required]
        public uint SignatureCounter { get; set; }

        [Required]
        [MaxLength(50)]
        public string CredType { get; set; } = null!; // e.g. "public-key"

        public Guid AaGuid { get; set; } // Device type GUID

        // Device Fingerprint
        [MaxLength(500)]
        public string? DeviceInfo { get; set; }

        [MaxLength(200)]
        public string? UserAgent { get; set; }

        [MaxLength(50)]
        public string? IpAddress { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        // Reset / Revoked
        public DateTime? RevokedAt { get; set; }
        public string? RevokedBy { get; set; }
        public string? RevokeReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
