using System.Data.Entity.ModelConfiguration;
using Domain.IdentityModels;

namespace DAL.EFConfiguration
{
    public class UserClaimMap : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("UserClaims");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ClaimType).HasColumnName("ClaimType");
            this.Property(t => t.ClaimValue).HasColumnName("ClaimValue");


        }
    }
}
