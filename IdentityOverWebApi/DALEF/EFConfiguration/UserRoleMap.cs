using System.Data.Entity.ModelConfiguration;
using Domain.IdentityModels;

namespace DALEF.EFConfiguration
{
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            // Primary Key
            HasKey(t => new { t.UserId, t.RoleId });

        }
    }
    public class UserRoleIntMap : EntityTypeConfiguration<UserRoleInt>
    {
        public UserRoleIntMap()
        {
            // Primary Key
            HasKey(t => new { t.UserId, t.RoleId });

        }
    }
}
