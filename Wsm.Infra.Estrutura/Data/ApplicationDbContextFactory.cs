using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wsm.Infra.Estrutura.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var host = Environment.GetEnvironmentVariable("DBHOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DBPORT") ?? "3306";
        var password = Environment.GetEnvironmentVariable("DBPASSWORD") ?? "numSey";

        var connectionString = $"server={host};port={port};user=root;password={password};database=controle_estoque";

        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
