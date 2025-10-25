using System.Collections.Generic;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Contracts.Services
{
    public interface IContactService
    {
        IContact GetContact(int id);
        IEnumerable<IContact> GetAllContacts();
        IEnumerable<IContact> GetContactsByLetter(char letter);
        IEnumerable<IContact> SearchContacts(string searchTerm);

        bool AddContact(IContact contact);
        bool UpdateContact(IContact contact);
        bool DeleteContact(int id);

        int GetContactCountByLetter(char letter);
        Dictionary<char, int> GetContactCountsByLetters();
        bool ValidateContact(IContact contact);
    }
}