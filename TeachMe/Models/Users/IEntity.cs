using System;

namespace TeachMe.Models.Users
{
    public interface IEntity
    {
        long CreationTicks { get; set; }
        Guid Id { get; set; }
    }
}