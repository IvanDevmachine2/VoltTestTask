using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VoltTry2.Contracts.Entities;
using VoltTry2.Contracts.Presenters;
using VoltTry2.Contracts.Services;

namespace VoltTry2.Presenters.Implementations
{
    public class MainPresenter : IMainPresenter
    {
        private readonly IContactService _contactService;
        private IEnumerable<IContact> _currentContacts;

        public MainPresenter(IContactService contactService)
        {
            _contactService = contactService;
        }

        // Добавляем метод для получения всех контактов
        public IEnumerable<IContact> GetAllContacts()
        {
            return _currentContacts ?? _contactService.GetAllContacts();
        }

        public void LoadContacts()
        {
                _currentContacts = _contactService.GetAllContacts();
                ContactsLoaded?.Invoke(_currentContacts);
                OperationCompleted?.Invoke("Контакты успешно загружены!");
                UpdateContactCounts();
        }

        public void LoadContactsByLetter(char letter)
        {
            try
            {
                _currentContacts = _contactService.GetContactsByLetter(letter);
                ContactsLoaded?.Invoke(_currentContacts);
                OperationCompleted?.Invoke($"Контакты на букву '{letter}' загружены");
            }
            catch (Exception ex)
            {
                OperationFailed?.Invoke($"Ошибка загрузки контактов: {ex.Message}");
            }
        }

        public void SearchContacts(string searchTerm)
        {
            try
            {
                _currentContacts = _contactService.SearchContacts(searchTerm);
                ContactsLoaded?.Invoke(_currentContacts);
                OperationCompleted?.Invoke($"Найдено {_currentContacts.Count()} контактов");
            }
            catch (Exception ex)
            {
                OperationFailed?.Invoke($"Ошибка поиска: {ex.Message}");
            }
        }

        public void AddContact(IContact contact)
        {
            try
            {
                if (_contactService.AddContact(contact))
                {
                    OperationCompleted?.Invoke("Контакт успешно добавлен");
                    UpdateContactCounts();
                    LoadContacts();
                }
                else
                {
                    OperationFailed?.Invoke("Не удалось добавить контакт. Проверьте данные.");
                }
            }
            catch (Exception ex)
            {
                OperationFailed?.Invoke($"Ошибка добавления контакта: {ex.Message}");
            }
        }

        public void UpdateContact(IContact contact)
        {
            try
            {
                if (_contactService.UpdateContact(contact))
                {
                    OperationCompleted?.Invoke("Контакт успешно обновлен");
                    UpdateContactCounts();
                    LoadContacts();
                }
                else
                {
                    OperationFailed?.Invoke("Не удалось обновить контакт. Проверьте данные.");
                }
            }
            catch (Exception ex)
            {
                OperationFailed?.Invoke($"Ошибка обновления контакта: {ex.Message}");
            }
        }

        //TODO: реализовать массовое удаление контактов
        public void DeleteContact(int contactId)
        {
            try
            {
                if (_contactService.DeleteContact(contactId))
                {
                    OperationCompleted?.Invoke("Контакт успешно удален");
                    UpdateContactCounts();
                    LoadContacts();
                }
                else
                {
                    OperationFailed?.Invoke("Не удалось удалить контакт");
                }
            }
            catch (Exception ex)
            {
                OperationFailed?.Invoke($"Ошибка удаления контакта: {ex.Message}");
            }
        }

        public void UpdateContactCounts()
        {
            try
            {
                var counts = _contactService.GetContactCountsByLetters();
                int subscriberCount = ContactCountsUpdated.GetInvocationList().Length;

                ContactCountsUpdated?.Invoke(counts);
            }
            catch (Exception ex)
            {
                OperationFailed?.Invoke($"Ошибка обновления счетчиков: {ex.Message}");
            }
        }

        // события
        public event Action<IEnumerable<IContact>> ContactsLoaded;
        public event Action<Dictionary<char, int>> ContactCountsUpdated;
        public event Action<string> OperationCompleted;
        public event Action<string> OperationFailed;
    }
}