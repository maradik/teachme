using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TeachMe.Models.Users;
using TeachMe.ProjectsSupport;

namespace TeachMe.Models.Jobs
{
    public class Job : IEntity
    {
        private List<JobAttachment> attachments;

        private static readonly ILookup<JobStatus, JobActionProjectTypePair> AvailableActionsForStatus = new[]
        {
            Tuple.Create(JobStatus.Draft, new JobActionProjectTypePair(JobActionType.Open, ProjectType.Student)),
            Tuple.Create(JobStatus.Opened, new JobActionProjectTypePair(JobActionType.Take, ProjectType.Teacher)),
            Tuple.Create(JobStatus.Opened, new JobActionProjectTypePair(JobActionType.Cancel, ProjectType.Student)),
            Tuple.Create(JobStatus.InWorking, new JobActionProjectTypePair(JobActionType.Finish, ProjectType.Teacher)),
            Tuple.Create(JobStatus.InReWorking, new JobActionProjectTypePair(JobActionType.Finish, ProjectType.Teacher)),
            Tuple.Create(JobStatus.Finished, new JobActionProjectTypePair(JobActionType.Accept, ProjectType.Student)),
            Tuple.Create(JobStatus.Finished, new JobActionProjectTypePair(JobActionType.Reject, ProjectType.Student))
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

        public JobActionType[] GetAvailableActions(ProjectType projectType)
        {
            return AvailableActionsForStatus.Contains(Status)
                       ? AvailableActionsForStatus[Status].Where(x => x.ProjectType == projectType)
                                                          .Select(x => x.ActionType)
                                                          .ToArray()
                       : new JobActionType[0];
        }

        public void DoAction(JobActionType actionType, ProjectType projectType)
        {
            if (!GetAvailableActions(projectType).Contains(actionType))
                throw new InvalidOperationException($"Недопустимое действие {actionType} над задачей {Id}, {nameof(projectType)}={projectType}");

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

        private class JobActionProjectTypePair
        {
            public JobActionProjectTypePair(JobActionType actionType, ProjectType projectType)
            {
                ActionType = actionType;
                ProjectType = projectType;
            }

            public JobActionType ActionType { get; }
            public ProjectType ProjectType { get; }
        }
    }
}