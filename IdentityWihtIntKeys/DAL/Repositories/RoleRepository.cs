using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityModels;

namespace DAL.Repositories
{
	public class RoleRepository : EFRepository<Role>, IRoleRepository
	{
		public RoleRepository(IDbContext dbContext) : base(dbContext)
		{
		}

	    public Role GetByRoleName(string roleName)
	    {
            return DbSet.FirstOrDefault(a => a.Name.ToUpper() == roleName.ToUpper());

	    }

	    public List<Role> GetRolesForUser(string userId)
	    {
            //var query = from userRole in _userRoles
            //            where userRole.UserId.Equals(userId)
            //            join role in _roleStore.DbEntitySet on userRole.RoleId equals role.Id
            //            select role.Name;


            //foreach (var role in ctx.Roles)
            //{
            //    foreach (var user in role.Users.Where(a => a.Id == id))
            //    {
            //        Console.WriteLine(role.Name);
            //    }
            //}

	        return (from role in DbSet from user in role.Users.Where(a => a.UserId == userId) select role).ToList();
	    }
	}
}
