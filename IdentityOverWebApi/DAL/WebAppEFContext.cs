using System;
using System.Data.Entity;
using DALWebApi.EFConfiguration;
using DALWebApi.Interfaces;
using Domain;
using Domain.IdentityModels;

namespace DALWebApi
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
            //Database.SetInitializer(new DropCreateDatabaseAlways<WebAppEFContext>());
        }

        public IDbSet<Value> Values { get; set; }


        // Identity tables, PK - string
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<UserClaim> UserClaims { get; set; }
        public IDbSet<UserLogin> UserLogins { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }

        // Identity tables, PK - int
        public IDbSet<RoleInt> RolesInt { get; set; }
        public IDbSet<UserClaimInt> UserClaimsInt { get; set; }
        public IDbSet<UserLoginInt> UserLoginsInt { get; set; }
        public IDbSet<UserInt> UsersInt { get; set; }
        public IDbSet<UserRoleInt> UserRolesInt { get; set; }

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
