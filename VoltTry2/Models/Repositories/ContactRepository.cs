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

            var allContacts = _dbSet.ToList();
            return allContacts
                .Where(c => !string.IsNullOrEmpty(c.LastName) &&
                            c.LastName.ToUpper().StartsWith(upperLetter))
                .Cast<IContact>();
        }

        public IEnumerable<IContact> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return GetAll();

            var allContacts = _dbSet.ToList();
            return allContacts.Where(c =>
                (c.LastName != null && c.LastName.Contains(searchTerm)) ||
                (c.FirstName != null && c.FirstName.Contains(searchTerm)) ||
                (c.MiddleName != null && c.MiddleName.Contains(searchTerm)) ||
                (c.PhoneNumber != null && c.PhoneNumber.Contains(searchTerm)) ||
                (c.Email != null && c.Email.Contains(searchTerm)))
                .Cast<IContact>();
        }

        public int GetCountByLetter(char letter)
        {
            var upperLetter = letter.ToString().ToUpper();

            // Выгружаем все контакты в память и считаем
            var allContacts = _dbSet.ToList();
            return allContacts.Count(c =>
                !string.IsNullOrEmpty(c.LastName) &&
                c.LastName.Length > 0 &&
                c.LastName.ToUpper()[0] == upperLetter[0]);
        }

        public Dictionary<char, int> GetCountsByLetters()
        {
            var counts = new Dictionary<char, int>();
            var alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

            // Выгружаем все фамилии в память
            var allContacts = _dbSet.ToList();
            var lastNames = allContacts.Select(c => c.LastName).Where(name => !string.IsNullOrEmpty(name)).ToList();

            foreach (char letter in alphabet)
            {
                var upperLetter = letter.ToString().ToUpper();
                counts[letter] = lastNames.Count(name =>
                    name.ToUpper()[0] == upperLetter[0]);
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