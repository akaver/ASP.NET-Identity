namespace DALWebApi.Interfaces
{
    public interface IDbContextFactory
    {
         IDbContext DbContext { get; }
    }
}
