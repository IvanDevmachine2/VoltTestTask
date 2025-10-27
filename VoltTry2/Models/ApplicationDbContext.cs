using System.Data.Entity;
using System.Data.Entity.Migrations;
using VoltTry2.Migrations;
using VoltTry2.Models.Entities;

namespace VoltTry2.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, MigrationsConfiguration>());
        }

        public DbSet<Contact> Contacts { get; set; }
    }
}