using System;
using TeachMe.Models;

namespace TeachMe.DataAccess
{
    public interface IJobRepository
    {
        Job Get(Guid id);
        Job GetByIdAndStudentUserId(Guid id, string studentUserId);
        Job[] GetAllByStudentUserId(string studentUserId);
        void Write(Job job);
        void Remove(Guid id);
    }
}