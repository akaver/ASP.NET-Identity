using DALWebApi.Interfaces;

namespace DALWebApi
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
