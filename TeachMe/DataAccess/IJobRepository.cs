using System;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public interface IJobRepository
    {
        Job Get(Guid id);
        void Write(Job job);
    }
}