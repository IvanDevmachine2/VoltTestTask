namespace VoltTry2.Contracts.Entities
{
    public interface IContact : IEntity
    {
        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string PhoneNumber { get; set; }
        string Email { get; set; }
        string Address { get; set; }
        string Notes { get; set; }

        // Определяемые функционалом свойства
        char FirstLetter { get; }
        string FullName { get; }
        string ShortName { get; }
    }
}