using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityModels;

namespace Identity
{
    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole> 
    {
        public CustomRoleStore(IUOW uow) : base(uow)
        {
        }
    }
}
