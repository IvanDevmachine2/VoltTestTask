using System.Collections.Generic;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Contracts.Repositories
{
    public interface IContactRepository : IRepository<IContact>
    {
        IEnumerable<IContact> GetByLastNameLetter(char letter);
        IEnumerable<IContact> Search(string searchTerm);
        int GetCountByLetter(char letter);
        Dictionary<char, int> GetCountsByLetters();
        bool PhoneNumberExists(string phoneNumber, int? excludeContactId = null);
    }
}