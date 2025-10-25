using System;
using System.Collections.Generic;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Contracts.Presenters
{
    public interface IMainPresenter
    {
        void LoadContacts();
        void LoadContactsByLetter(char letter);
        void SearchContacts(string searchTerm);
        void AddContact(IContact contact);
        void UpdateContact(IContact contact);
        void DeleteContact(int contactId);
        void UpdateContactCounts();

        IEnumerable<IContact> GetAllContacts();

        event Action<IEnumerable<IContact>> ContactsLoaded;
        event Action<Dictionary<char, int>> ContactCountsUpdated;
        event Action<string> OperationCompleted;
        event Action<string> OperationFailed;
    }
}