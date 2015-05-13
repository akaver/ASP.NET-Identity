using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.IdentityModels;

namespace Domain
{
    /// <summary>
    /// This is dummy demo POCO, to show relationship with Identity pocos
    /// </summary>
    public class Value
    {
        public int ValueId { get; set; }
        public string Name { get; set; }

        public int UserIntId { get; set; }
        public virtual UserInt UserInt { get; set; }
    }
}
