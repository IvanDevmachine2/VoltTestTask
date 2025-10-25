using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using VoltTry2.Models.Entities;

namespace VoltTry2.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
            // Для SQLite используем специальный инициализатор
            Database.SetInitializer(new SQLiteDatabaseInitializer());
        }

        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // Конфигурация Contact
            modelBuilder.Entity<Contact>()
                .ToTable("Contacts")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Contact>()
                .Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Contact>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Contact>()
                .Property(c => c.MiddleName)
                .IsOptional()
                .HasMaxLength(50);

            modelBuilder.Entity<Contact>()
                .Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Email)
                .IsOptional()
                .HasMaxLength(100);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Address)
                .IsOptional()
                .HasMaxLength(200);

            modelBuilder.Entity<Contact>()
                .Property(c => c.Notes)
                .IsOptional();

            modelBuilder.Entity<Contact>()
                .Property(c => c.CreatedAt)
                .IsRequired();

            modelBuilder.Entity<Contact>()
                .Property(c => c.UpdatedAt)
                .IsOptional();
        }
    }

    // инициализация SQLite
    public class SQLiteDatabaseInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            try
            {
                // проверка существования бд в каталоге, если её нет:
                if (!context.Database.Exists())
                {
                    // создание бд
                    context.Database.Create();

                    // добавление тестовых данных
                    SeedTestData(context);
                }
            }
            catch
            {
                // тут можно добавить обработку ошибок
            }
        }

        private void SeedTestData(ApplicationDbContext context)
        {
            try
            {
                context.Contacts.AddRange(new[]
                {
                    new Contact
                    {
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        PhoneNumber = "+79161234567",
                        Email = "ivanov@mail.ru",
                        Address = "Москва, ул. Ленина, д. 1",
                        CreatedAt = DateTime.Now
                    },
                    new Contact
                    {
                        LastName = "Петров",
                        FirstName = "Петр",
                        MiddleName = "Петрович",
                        PhoneNumber = "+79161234568",
                        Email = "petrov@gmail.com",
                        Address = "Санкт-Петербург, Невский пр., д. 10",
                        CreatedAt = DateTime.Now
                    }
                });
                context.SaveChanges();
            }
            catch
            {
                // тут можно добавить обработку ошибок
            }
        }
    }
}