using System;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public interface IJobRepository
    {
        Job Get(Guid id);
        Job[] GetAllByStudentUserId(string studentUserId);
        void Write(Job job);
        void Remove(Guid id);
    }
}