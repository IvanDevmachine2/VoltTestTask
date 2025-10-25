using System;

namespace VoltTry2.Contracts.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }

    public interface IEntity : IEntity<int>
    {
    }
}