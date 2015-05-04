using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Domain.IdentityModels;

namespace Identity
{
    public class CustomUserStore : UserStore<CustomUser, CustomRole, int,
    CustomUserLogin, CustomUserRole, CustomUserClaim> 
    {
        public CustomUserStore(IUOW uow) : base(uow)
        {
        }
    }
}
