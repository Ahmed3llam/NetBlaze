using NetBlaze.Domain.Common;
using NetBlaze.Domain.Entities.Identity;

namespace NetBlaze.Domain.Entities
{
    public class Department : BaseEntity<long>
    {
        // Properties

        public string Name { get; set; }


        // Navigational Properties

        public ICollection<User> Users { get; set; } = [];
    }
}
