using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Models.Entities
{
    public class Contact : BaseEntity, IContact
    {
        [Required]
        [MaxLength(50)]
        public virtual string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [MaxLength(50)]
        public virtual string MiddleName { get; set; }

        [Required]
        [MaxLength(20)]
        public virtual string PhoneNumber { get; set; }

        [MaxLength(100)]
        public virtual string Email { get; set; }

        [MaxLength(200)]
        public virtual string Address { get; set; }

        public virtual string Notes { get; set; }

        [NotMapped]
        public virtual char FirstLetter => char.ToUpper(LastName?[0] ?? 'А');

        [NotMapped]
        public virtual string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();

        [NotMapped]
        public virtual string ShortName =>
            $"{LastName} {FirstName?[0]}.{(!string.IsNullOrEmpty(MiddleName) ? MiddleName[0] + "." : "")}";
    }
}