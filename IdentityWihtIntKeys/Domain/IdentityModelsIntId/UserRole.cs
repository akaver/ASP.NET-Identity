using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IdentityModelsIntId
{
    public class UserRole : UserRole<int>
    {
    }

    public class UserRole<TKey>
    {
        public TKey UserId { get; set; }
        public virtual User User { get; set; }

        public TKey RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
