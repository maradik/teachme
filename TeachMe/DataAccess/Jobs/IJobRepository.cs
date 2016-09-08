﻿using System;
using TeachMe.Models.Jobs;

namespace TeachMe.DataAccess.Jobs
{
    public interface IJobRepository
    {
        Job Get(Guid id);
        Job GetByIdAndStudentUserId(Guid id, string studentUserId);
        Job GetByIdAndTeacherUserId(Guid id, string teacherUserId);
        Job[] GetAllByStudentUserId(string studentUserId);
        Job[] GetAllByTeacherUserId(string teacherUserId);
        Job[] GetAllByStatus(JobStatus status);
        void Write(Job job);
        void Remove(Guid id);
    }
}