using System.Data.Entity.Migrations;
using VoltTry2.Models;
using VoltTry2.Models.Entities;

namespace VoltTry2.Migrations
{
    internal sealed class MigrationsConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}