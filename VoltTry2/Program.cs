using System;
using System.Data.Entity;
using System.Windows.Forms;
using VoltTry2.Contracts.Presenters;
using VoltTry2.Contracts.Repositories;
using VoltTry2.Contracts.Services;
using VoltTry2.Forms;
using VoltTry2.Models;
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
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, VoltTry2.Migrations.MigrationsConfiguration>());

                var dbContext = new ApplicationDbContext();

                IContactRepository contactRepository = new ContactRepository(dbContext);
                IContactService contactService = new ContactService(contactRepository);
                IMainPresenter mainPresenter = new MainPresenter(contactService);

                var mainForm = new MainForm();

                // связь презентера с формой
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

                // передача презентера в форму
                mainForm.Presenter = mainPresenter;

                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка запуска приложения: {ex.Message}\n\nДетали: {ex.InnerException?.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}