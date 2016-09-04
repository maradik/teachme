using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TeachMe.Models.Users;

namespace TeachMe.Models.Jobs
{
    public class Job : IEntity
    {
        private List<JobAttachment> attachments;

        private static readonly ILookup<JobStatus, JobActionByUserRole> AvailableActionsForStatus = new[]
        {
            Tuple.Create(JobStatus.Draft, new JobActionByUserRole(JobActionType.Open, UserRole.Student)),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Take, UserRole.Teacher)),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Cancel, UserRole.Student)),
            Tuple.Create(JobStatus.InWorking, new JobActionByUserRole(JobActionType.Finish, UserRole.Teacher)),
            Tuple.Create(JobStatus.InReWorking, new JobActionByUserRole(JobActionType.Finish, UserRole.Teacher)),
            Tuple.Create(JobStatus.Finished, new JobActionByUserRole(JobActionType.Accept, UserRole.Student)),
            Tuple.Create(JobStatus.Finished, new JobActionByUserRole(JobActionType.Reject, UserRole.Student))
        }.ToLookup(x => x.Item1, x => x.Item2);

        public Guid Id { get; set; }

        [Display(Name = "Предмет")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите значение из списка")]
        public int SubjectId { get; set; }

        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Статус")]
        public JobStatus Status { get; set; }

        [Display(Name = "Стоимость")]
        public decimal Cost { get; set; }

        [DisplayName("Фото, документы")]
        public List<JobAttachment> Attachments
        {
            get { return attachments ?? (attachments = new List<JobAttachment>()); }
            set { attachments = value; }
        }

        [DisplayName("Исполнитель")]
        public string TeacherUserId { get; set; }

        [DisplayName("Заказчик")]
        public string StudentUserId { get; set; }

        [DisplayName("Срок исполнения")]
        public long DeadlineTicks { get; set; }

        [DisplayName("Дата создания")]
        public long CreationTicks { get; set; }

        public JobActionType[] GetAvailableActions(ApplicationUser user)
        {
            return AvailableActionsForStatus.Contains(Status)
                       ? AvailableActionsForStatus[Status].Where(x => user.Roles.Contains(x.UserRole.Name))
                                                          .Select(x => x.ActionType)
                                                          .ToArray()
                       : new JobActionType[0];
        }

        public void DoAction(JobActionType actionType, ApplicationUser user)
        {
            if (!GetAvailableActions(user).Contains(actionType))
                throw new InvalidOperationException($"Недопустимое действие {actionType} над задачей {Id}, {nameof(user)}={user.Id}");

            switch (actionType)
            {
                case JobActionType.Finish:
                    Status = JobStatus.Finished;
                    break;
                case JobActionType.Accept:
                    Status = JobStatus.Accepted;
                    break;
                case JobActionType.Cancel:
                    Status = JobStatus.Cancelled;
                    break;
                case JobActionType.Reject:
                    Status = JobStatus.InReWorking;
                    break;
                default:
                    throw new NotImplementedException($"Неизвестное действие {actionType}");
            }
        }

        private class JobActionByUserRole
        {
            public JobActionByUserRole(JobActionType actionType, UserRole userRole)
            {
                ActionType = actionType;
                UserRole = userRole;
            }

            public JobActionType ActionType { get; }
            public UserRole UserRole { get; }
        }
    }
}