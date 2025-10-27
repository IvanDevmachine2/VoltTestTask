using System.Collections.Generic;
using System.Linq;
using VoltTry2.Contracts.Entities;
using VoltTry2.Contracts.Repositories;
using VoltTry2.Models.Entities;

namespace VoltTry2.Models.Repositories
{
    public class ContactRepository : BaseRepository<Contact, IContact, ApplicationDbContext>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<IContact> GetByLastNameLetter(char letter)
        {
            var upperLetter = letter.ToString().ToUpper();

            return _dbSet
                .Where(c => c.LastName != null && c.LastName.ToUpper().StartsWith(upperLetter))
                .AsEnumerable()
                .Cast<IContact>();
        }

        public IEnumerable<IContact> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAll();

            return _dbSet
                .Where(c =>
                    (c.LastName != null && c.LastName.Contains(searchTerm)) ||
                    (c.FirstName != null && c.FirstName.Contains(searchTerm)) ||
                    (c.MiddleName != null && c.MiddleName.Contains(searchTerm)) ||
                    (c.PhoneNumber != null && c.PhoneNumber.Contains(searchTerm)) ||
                    (c.Email != null && c.Email.Contains(searchTerm)))
                .AsEnumerable()
                .Cast<IContact>();
        }

        public int GetCountByLetter(char letter)
        {
            var upperLetter = letter.ToString().ToUpper();

            return _dbSet
                .Count(c => c.LastName != null &&
                           c.LastName.Length > 0 &&
                           c.LastName.ToUpper().StartsWith(upperLetter));
        }

        public Dictionary<char, int> GetCountsByLetters()
        {
            var counts = new Dictionary<char, int>();
            var alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

            var dbCounts = _dbSet
                .Where(c => c.LastName != null && c.LastName.Length > 0)
                .AsEnumerable()
                .GroupBy(c => c.LastName.ToUpper()[0])
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (char letter in alphabet)
            {
                var upperLetter = char.ToUpper(letter);
                counts[letter] = dbCounts.ContainsKey(upperLetter) ? dbCounts[upperLetter] : 0;
            }

            return counts;
        }

        public bool PhoneNumberExists(string phoneNumber, int? excludeContactId = null)
        {
            var query = _dbSet.Where(c => c.PhoneNumber == phoneNumber);

            if (excludeContactId.HasValue)
            {
                query = query.Where(c => c.Id != excludeContactId.Value);
            }

            return query.Any();
        }
    }
}