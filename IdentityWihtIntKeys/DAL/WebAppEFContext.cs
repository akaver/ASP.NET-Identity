using System;
using System.Data.Entity;
using DAL.EFConfiguration;
using DAL.Interfaces;
using Domain.IdentityModels;

namespace DAL
{
    public class WebAppEFContext : DbContext, IDbContext, IDisposable
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string _instanceId = Guid.NewGuid().ToString();

        public WebAppEFContext()
			: base("Name=WebAppNoEFConnection")
        {
			_logger.Info("_instanceId: " + _instanceId);
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<WebAppEFContext>());
        }

        // Identity tables, PK - string
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // Identity tables, PK - int
        public DbSet<RoleInt> RolesInt { get; set; }
        public DbSet<UserClaimInt> UserClaimsInt { get; set; }
        public DbSet<UserLoginInt> UserLoginsInt { get; set; }
        public DbSet<UserInt> UsersInt { get; set; }
        public DbSet<UserRoleInt> UserRolesInt { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Identity, PK - string 
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UserClaimMap());
            modelBuilder.Configurations.Add(new UserLoginMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserRoleMap());

            // Identity, PK - int 
            modelBuilder.Configurations.Add(new RoleIntMap());
            modelBuilder.Configurations.Add(new UserClaimIntMap());
            modelBuilder.Configurations.Add(new UserLoginIntMap());
            modelBuilder.Configurations.Add(new UserIntMap());
            modelBuilder.Configurations.Add(new UserRoleIntMap());
        }

	    protected override void Dispose(bool disposing)
	    {
			_logger.Info("Disposing: " + disposing + " _instanceId: " + _instanceId);
		    base.Dispose(disposing);
	    }
    }
}
