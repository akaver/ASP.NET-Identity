using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.IdentityModels;

namespace WebAppNoEF.ViewModels
{
    //http://stackoverflow.com/questions/10092899/asp-net-mvc2-system-missingmethodexception-no-parameterless-constructor-defin
    [Bind(Exclude = "UserSelectList,RoleSelectList")]
    public class EditViewModel
    {
        public UserRole UserRole { get; set; }
        public UserRole OriginalUserRole { get; set; }
        [DisplayName("User")]
        public SelectList UserSelectList { get; set; }
        [DisplayName("Role")]
        public SelectList RoleSelectList { get; set; }
    }


}