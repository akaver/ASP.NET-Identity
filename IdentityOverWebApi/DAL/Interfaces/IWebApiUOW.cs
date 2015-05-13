using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALWebApi.Interfaces
{
    public interface IWebApiUOW
    {
        //save pending changes to the data store
        void Commit();

        IWebApiValueRepository Values { get; }
        void SetSecurity(string securityToken);
    }
}
