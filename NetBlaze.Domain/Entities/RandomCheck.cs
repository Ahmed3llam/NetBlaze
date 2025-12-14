using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class RandomCheck : BaseEntity<long>
    {
        // Properties

        public DateOnly Date { get; set; }

        public TimeSpan Time { get; set; }

        public string OTP { get; set; }

        public long UserId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsReplied { get; set; } = false;

        public DateTime? RepliedDate { get; set; }

        // Navigational Properties

        public virtual User User { get; set; }

        public ICollection<AttendancePolicyAction> AttendancePolicyActions { get; set; } = [];
    }
}
