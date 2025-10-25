using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VoltTry2.Contracts.Presenters;
using VoltTry2.Contracts.Repositories;
using VoltTry2.Contracts.Services;
using VoltTry2.Forms;
using VoltTry2.Models;
using VoltTry2.Models.Entities;
using VoltTry2.Models.Repositories;
using VoltTry2.Presenters.Implementations;
using VoltTry2.Services.Implementations;

namespace VoltTry2
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Устанавливаем путь к данным
                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

                // Создаем контекст и инициализируем БД
                var dbContext = new ApplicationDbContext();
                InitializeDatabase(dbContext);

                IContactRepository contactRepository = new ContactRepository(dbContext);
                IContactService contactService = new ContactService(contactRepository);
                IMainPresenter mainPresenter = new MainPresenter(contactService);

                var mainForm = new MainForm();

                // связываем презентер с формой
                mainPresenter.ContactsLoaded += contacts => mainForm.DisplayContacts(contacts);
                mainPresenter.ContactCountsUpdated += counts =>
                {
                    foreach (var kvp in counts)
                    {
                        mainForm.UpdateContactCount(kvp.Key, kvp.Value);
                    }
                };
                mainPresenter.OperationCompleted += message => MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mainPresenter.OperationFailed += message => MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Передаем презентер в форму
                mainForm.Presenter = mainPresenter;

                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска приложения: {ex.Message}\n\nДетали: {ex.InnerException?.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void InitializeDatabase(ApplicationDbContext context)
        {
            try
            {
                var dbPath = Path.Combine(Application.StartupPath, "PhoneBook.db");

                // 1. Если файла БД нет - создаем его с таблицами
                if (!File.Exists(dbPath))
                {
                    CreateDatabaseWithTables(context);
                    return;
                }

                // 2. Если файл есть - проверяем есть ли таблица Contacts
                if (!TableExists(context))
                {
                    // Таблицы нет - создаем её ЖЕЛЕЗОБЕТОННЫМ способом
                    CreateTablesIronclad(context);
                    return;
                }

                // 3. Проверяем и добавляем тестовые данные если нужно
                EnsureTestData(context);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static void CreateDatabaseWithTables(ApplicationDbContext context)
        {
            try
            {
                // Создаем БД
                context.Database.Create();

                // Создаем таблицу явно через SQL
                CreateContactsTable(context);

                SeedTestData(context);
                MessageBox.Show("База данных создана успешно!", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания БД: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static bool TableExists(ApplicationDbContext context)
        {
            try
            {
                // Проверяем через системные таблицы SQLite
                var sql = "SELECT 1 FROM sqlite_master WHERE type='table' AND name='Contacts'";
                var result = context.Database.SqlQuery<int>(sql).FirstOrDefault();
                return result == 1;
            }
            catch
            {
                return false;
            }
        }

        private static void CreateTablesIronclad(ApplicationDbContext context)
        {
            try
            {
                // ЖЕЛЕЗОБЕТОННЫЙ СПОСОБ: создаем таблицу через прямой SQL
                CreateContactsTable(context);

                MessageBox.Show("Таблица Contacts создана успешно!", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось создать таблицу: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static void CreateContactsTable(ApplicationDbContext context)
        {
            // Прямой SQL для создания таблицы
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Contacts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    LastName NVARCHAR(50) NOT NULL,
                    FirstName NVARCHAR(50) NOT NULL,
                    MiddleName NVARCHAR(50),
                    PhoneNumber NVARCHAR(20) NOT NULL,
                    Email NVARCHAR(100),
                    Address NVARCHAR(200),
                    Notes TEXT,
                    CreatedAt DATETIME NOT NULL,
                    UpdatedAt DATETIME
                );";

            context.Database.ExecuteSqlCommand(createTableSql);
        }

        private static void EnsureTestData(ApplicationDbContext context)
        {
            try
            {
                // Добавляем тестовые данные только если таблица пустая
                if (!context.Contacts.Any())
                {
                    SeedTestData(context);
                }
            }
            catch (Exception ex)
            {
                // Не критичная ошибка - просто логируем
                Console.WriteLine($"Ошибка добавления тестовых данных: {ex.Message}");
            }
        }

        private static void SeedTestData(ApplicationDbContext context)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка заполнения тестовыми данными: {ex.Message}", "Предупреждение",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}