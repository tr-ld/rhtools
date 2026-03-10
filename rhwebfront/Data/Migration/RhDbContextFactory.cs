using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RHWebFront.Data.Migration;

public class RhDbContextFactory : IDesignTimeDbContextFactory<RhDbContext>
{
    public RhDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RhDbContext>();
        optionsBuilder.UseSqlite("Data Source=rhtools.db")
                      .ReplaceService<IMigrationsSqlGenerator, RhToolsSqliteMigrationsSqlGenerator>();

        return new RhDbContext(optionsBuilder.Options);
    }
}