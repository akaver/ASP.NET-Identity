using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IDbContext _context;

        public DbContextFactory()
        {
            _context = new WebAppEFContext();
        }

        public IDbContext DbContext {
            get { return _context; }
        }
       
    }
}
