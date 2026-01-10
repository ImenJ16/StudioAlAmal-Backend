using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CommunicationService.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommunicationDbContext>
    {
        public CommunicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CommunicationDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=StudioAlAmalDB;Trusted_Connection=True;TrustServerCertificate=True;"
            );

            return new CommunicationDbContext(optionsBuilder.Options);
        }
    }
}