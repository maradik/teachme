using System;

namespace TeachMe.Models
{
    public interface IEntity<TId> where TId : struct, IComparable
    {
        TId Id { get; set; }
        long CreationTicks { get; set; }
    }
}