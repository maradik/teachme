﻿using System;
using System.Linq;
using TeachMe.DataAccess;
using TeachMe.Models.Jobs;
using TeachMe.Models.Users;

namespace TeachMe.Services.Jobs
{
    public class JobActionService : IJobActionService
    {
        private readonly IJobRepository jobRepository;

        public JobActionService(IJobRepository jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        private static readonly ILookup<JobStatus, JobActionByUserRole> AvailableActionsForStatus = new[]
        {
            Tuple.Create(JobStatus.Draft, new JobActionByUserRole(JobActionType.Open, UserRole.Student, (j, u) => j.StudentCost <= u.Cash.AvailableAmount)),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Hide, UserRole.Student)),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Take, UserRole.Teacher)),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Cancel, UserRole.Student)),
            Tuple.Create(JobStatus.InWorking, new JobActionByUserRole(JobActionType.Finish, UserRole.Teacher)),
            Tuple.Create(JobStatus.InReWorking, new JobActionByUserRole(JobActionType.Finish, UserRole.Teacher)),
            Tuple.Create(JobStatus.Finished, new JobActionByUserRole(JobActionType.Accept, UserRole.Student)),
            Tuple.Create(JobStatus.Finished, new JobActionByUserRole(JobActionType.Reject, UserRole.Student))
        }.ToLookup(x => x.Item1, x => x.Item2);

        public JobActionType[] GetAvailableActions(Job job, ApplicationUser user)
        {
            return AvailableActionsForStatus.Contains(job.Status)
                       ? AvailableActionsForStatus[job.Status].Where(x => user.Roles.Contains(x.UserRole.Name))
                                                              .Where(x => x.UserRole == UserRole.Teacher ? IsTeacherHasAccess(job, user) : IsStudentHasAccess(job, user))
                                                              .Where(x => x.CheckForAvailability(job, user))
                                                              .Select(x => x.ActionType)
                                                              .ToArray()
                       : new JobActionType[0];
        }

        public Job DoAction(Guid jobId, JobActionType actionType, ApplicationUser user)
        {
            var job = jobRepository.Get(jobId);

            if (!GetAvailableActions(job, user).Contains(actionType))
                throw new InvalidOperationException($"Недопустимое действие {actionType} над задачей {job.Id}, {nameof(user)}={user.Id}");

            switch (actionType)
            {
                case JobActionType.Hide:
                    job.Status = JobStatus.Draft;
                    break;
                case JobActionType.Open:
                    job.Status = JobStatus.Opened;
                    break;
                case JobActionType.Take:
                    job.Status = JobStatus.InWorking;
                    job.TeacherUserId = user.Id;
                    break;
                case JobActionType.Finish:
                    job.Status = JobStatus.Finished;
                    break;
                case JobActionType.Accept:
                    job.Status = JobStatus.Accepted;
                    break;
                case JobActionType.Cancel:
                    job.Status = JobStatus.Cancelled;
                    break;
                case JobActionType.Reject:
                    job.Status = JobStatus.InReWorking;
                    break;
                default:
                    throw new NotImplementedException($"Неизвестное действие {actionType}");
            }

            jobRepository.Write(job);

            return job;
        }

        public bool IsStudentHasAccess(Job job, ApplicationUser user)
        {
            return job.StudentUserId == user.Id;
        }

        public bool IsTeacherHasAccess(Job job, ApplicationUser user)
        {
            return (string.IsNullOrEmpty(job.TeacherUserId) || job.TeacherUserId == user.Id) && job.StudentUserId != user.Id;
        }

        private class JobActionByUserRole
        {
            public JobActionByUserRole(JobActionType actionType, UserRole userRole, Func<Job, ApplicationUser, bool> checkForAvailability = null)
            {
                ActionType = actionType;
                UserRole = userRole;
                CheckForAvailability = checkForAvailability ?? ((job, user) => true);
            }

            public JobActionType ActionType { get; }
            public UserRole UserRole { get; }
            public Func<Job, ApplicationUser, bool> CheckForAvailability { get; }
        }
    }
}