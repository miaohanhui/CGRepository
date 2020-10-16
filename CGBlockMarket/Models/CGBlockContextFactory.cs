using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CGBlockMarket
{
    //You haven't a public constructor without parameters in your database class, so you need to tell a compiler how to create an instance of your database.
    //Just add a new class:
    public class CGBlockContextFactory : IDesignTimeDbContextFactory<CGBlockContext>
    {
        public CGBlockContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CGBlockContext>();
            var connectionString = configuration.GetSection("ConnectionString")["Connection"];

            builder.UseSqlServer(connectionString);

            return new CGBlockContext(builder.Options);
        }
    }
}
