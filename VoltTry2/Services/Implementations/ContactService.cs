using System;
using System.Collections.Generic;
using System.Linq;
using VoltTry2.Contracts.Entities;
using VoltTry2.Contracts.Repositories;
using VoltTry2.Contracts.Services;
using VoltTry2.Models.Entities;

namespace VoltTry2.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public IContact GetContact(int id)
        {
            return _contactRepository.GetById(id);
        }

        public IEnumerable<IContact> GetAllContacts()
        {
            return _contactRepository.GetAll();
        }

        public IEnumerable<IContact> GetContactsByLetter(char letter)
        {
            return _contactRepository.GetByLastNameLetter(letter);
        }

        public IEnumerable<IContact> SearchContacts(string searchTerm)
        {
            return _contactRepository.Search(searchTerm);
        }

        public bool AddContact(IContact contact)
        {
            if (!ValidateContact(contact))
                return false;

            // Проверяем уникальность номера телефона
            if (_contactRepository.PhoneNumberExists(contact.PhoneNumber))
                return false;

            _contactRepository.Add(contact);
            return true;
        }

        public bool UpdateContact(IContact contact)
        {
            if (!ValidateContact(contact))
                return false;

            // Проверяем уникальность номера телефона (без текущего контакта)
            if (_contactRepository.PhoneNumberExists(contact.PhoneNumber, contact.Id))
                return false;

            var existingContact = _contactRepository.GetById(contact.Id);
            if (existingContact == null)
                return false;

            _contactRepository.Update(contact);
            return true;
        }

        public bool DeleteContact(int id)
        {
            var contact = _contactRepository.GetById(id);
            if (contact == null)
                return false;

            _contactRepository.Remove(contact);
            return true;
        }

        public int GetContactCountByLetter(char letter)
        {
            return _contactRepository.GetCountByLetter(letter);
        }

        public Dictionary<char, int> GetContactCountsByLetters()
        {
            return _contactRepository.GetCountsByLetters();
        }

        public bool ValidateContact(IContact contact)
        {
            if (contact == null)
                return false;

            if (string.IsNullOrWhiteSpace(contact.LastName) || contact.LastName.Length > 50)
                return false;

            if (string.IsNullOrWhiteSpace(contact.FirstName) || contact.FirstName.Length > 50)
                return false;

            if (string.IsNullOrWhiteSpace(contact.PhoneNumber) || contact.PhoneNumber.Length > 20)
                return false;

            if (contact.MiddleName?.Length > 50)
                return false;

            if (contact.Email?.Length > 100)
                return false;

            if (contact.Address?.Length > 200)
                return false;

            return true;
        }
    }
}