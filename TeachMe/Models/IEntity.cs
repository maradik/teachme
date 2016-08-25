using System;

namespace TeachMe.Models
{
    public interface IEntity
    {
        long CreationTicks { get; set; }
        Guid Id { get; set; }
    }
}