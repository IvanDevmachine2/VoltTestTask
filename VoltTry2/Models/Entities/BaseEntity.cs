using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VoltTry2.Contracts.Entities;

namespace VoltTry2.Models.Entities
{
    public abstract class BaseEntity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual DateTime? UpdatedAt { get; set; }

        public virtual void UpdateTimestamps()
        {
            UpdatedAt = DateTime.Now;
            if (CreatedAt == default)
                CreatedAt = DateTime.Now;
        }
    }
}