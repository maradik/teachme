using log4net;
using System;
using System.Linq;
using TeachMe.DataAccess.FileUploading;
using TeachMe.DataAccess.Jobs;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.Models.Jobs;
using TeachMe.Models.Transactions;
using TeachMe.Models.Users;
using TeachMe.Services.UserCasheSupport;

namespace TeachMe.Services.Jobs
{
    public class JobActionService : IJobActionService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(JobActionService));

        private readonly IJobRepository jobRepository;
        private readonly IUploadedFileRepository uploadedFileRepository;
        private readonly IUserCashOperationService cashOperationService;
        private readonly IJobActionCustomHandler[] customHandlers;

        public JobActionService(IJobRepository jobRepository,
                                IUploadedFileRepository uploadedFileRepository,
                                IUserCashOperationService cashOperationService,
                                IJobActionCustomHandler[] customHandlers)
        {
            this.jobRepository = jobRepository;
            this.uploadedFileRepository = uploadedFileRepository;
            this.cashOperationService = cashOperationService;
            this.customHandlers = customHandlers;
        }

        private static readonly ILookup<JobStatus, JobActionByUserRole> AvailableActionsForStatus = new[]
        {
            Tuple.Create(JobStatus.Draft, new JobActionByUserRole(JobActionType.Open, UserRole.Student, new ISpecification<Job>[] {JobOpeningSpecification.Instance})),
            Tuple.Create(JobStatus.Draft, new JobActionByUserRole(JobActionType.Edit, UserRole.Student, new ISpecification<Job>[] {JobEditingSpecification.Instance})),
            Tuple.Create(JobStatus.Draft, new JobActionByUserRole(JobActionType.Delete, UserRole.Student, new ISpecification<Job>[] {JobDeletionSpecification.Instance})),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Hide, new [] {UserRole.Student, UserRole.Admin})),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Take, UserRole.Teacher)),
            Tuple.Create(JobStatus.Opened, new JobActionByUserRole(JobActionType.Cancel, UserRole.Student)),
            Tuple.Create(JobStatus.Cancelled, new JobActionByUserRole(JobActionType.Delete, UserRole.Student, new ISpecification<Job>[] {JobDeletionSpecification.Instance})),
            Tuple.Create(JobStatus.InWorking, new JobActionByUserRole(JobActionType.Finish, UserRole.Teacher)),
            Tuple.Create(JobStatus.InWorking, new JobActionByUserRole(JobActionType.OfferAbort, new [] {UserRole.Student, UserRole.Admin})),
            Tuple.Create(JobStatus.AbortOffered, new JobActionByUserRole(JobActionType.ConfirmAbort, new [] {UserRole.Teacher, UserRole.Admin})),
            Tuple.Create(JobStatus.AbortOffered, new JobActionByUserRole(JobActionType.RejectAbort, UserRole.Teacher)),
            Tuple.Create(JobStatus.InReWorking, new JobActionByUserRole(JobActionType.Finish, UserRole.Teacher)),
            Tuple.Create(JobStatus.InReWorking, new JobActionByUserRole(JobActionType.OfferAbort, new [] {UserRole.Student, UserRole.Admin})),
            Tuple.Create(JobStatus.Finished, new JobActionByUserRole(JobActionType.Accept, new [] {UserRole.Student, UserRole.Admin})),
            Tuple.Create(JobStatus.Finished, new JobActionByUserRole(JobActionType.Reject, UserRole.Student)),
            Tuple.Create(JobStatus.FinishedWithRemainAmountNeeded, new JobActionByUserRole(JobActionType.ReserveRemainAmount, new [] {UserRole.Student, UserRole.Admin}, new ISpecification<Job>[] {JobReservingRemainAmountSpecification.Instance})),
            Tuple.Create(JobStatus.FinishedWithRemainAmountNeeded, new JobActionByUserRole(JobActionType.AcceptWithoutRemainAmount, UserRole.Admin)),
            Tuple.Create(JobStatus.InArbitrage, new JobActionByUserRole(JobActionType.Accept, UserRole.Admin)),
            Tuple.Create(JobStatus.InArbitrage, new JobActionByUserRole(JobActionType.ConfirmAbort, UserRole.Admin))
        }.ToLookup(x => x.Item1, x => x.Item2);

        public JobActionType[] GetAvailableActions(Job job, ApplicationUser user)
        {
            return AvailableActionsForStatus.Contains(job.Status)
                       ? AvailableActionsForStatus[job.Status].Where(x => x.IsUserHasAccess(user, job))
                                                              .Where(x => x.Specifications.Length == 0 || x.Specifications.All(y => y.IsSatisfiedBy(job)))
                                                              .Select(x => x.ActionType)
                                                              .ToArray()
                       : new JobActionType[0];
        }

        public Job DoAction(Guid jobId, JobActionType actionType, ApplicationUser user)
        {
            var job = jobRepository.Get(jobId);

            if (!GetAvailableActions(job, user).Contains(actionType))
                throw new InvalidJobActionException($"Недопустимое действие {actionType} над задачей {job.Id}, {nameof(user)}={user.Id}");

            foreach (var customHandler in customHandlers)
            {
                try
                {
                    customHandler.Handle(job, actionType, user);
                }
                catch (Exception e)
                {
                    Logger.Error($"Обработчик {customHandler.GetType()} рассыпался с ошибкой", e);
                }
            }

            switch (actionType)
            {
                case JobActionType.Hide:
                    job.Status = JobStatus.Draft;
                    cashOperationService.UnfreezeUserMoney(job.StudentUserId, job.StudentPrepaymentAmount);
                    job.StudentPrepaymentAmount = 0;
                    break;
                case JobActionType.Open:
                    job.Status = JobStatus.Opened;
                    job.StudentPrepaymentAmount = Math.Min(job.StudentCost, job.GetStudentUser().Cash.AvailableAmount);
                    cashOperationService.FreezeUserMoney(job.StudentUserId, job.StudentPrepaymentAmount);
                    break;
                case JobActionType.Take:
                    if (job.StudentUserId == user.Id)
                    {
                        throw new InvalidOperationException($"Нельзя быть одновременно заказчиком и исполнителем в одной задаче! Пользователь: {user.UserName}");
                    }
                    job.Status = JobStatus.InWorking;
                    job.TeacherUserId = user.Id;
                    var prepaymentTransactionDescription = $"Предоплата по задаче {job.Title}";
                    cashOperationService.TransferMoneyFromUserToUser(job.StudentUserId, job.TeacherUserId, job.StudentPrepaymentAmount, job.PrepaymentCommission, TransactionType.JobPrepayment, prepaymentTransactionDescription);
                    cashOperationService.FreezeUserMoney(job.TeacherUserId, job.TeacherPrepaymentAmount);
                    cashOperationService.UnfreezeUserMoney(job.StudentUserId, job.StudentPrepaymentAmount);
                    break;
                case JobActionType.Finish:
                    job.Status = job.GetStudentRemainAmount() > 0 ? JobStatus.FinishedWithRemainAmountNeeded : JobStatus.Finished;
                    break;
                case JobActionType.ReserveRemainAmount:
                    job.Status = JobStatus.Finished;
                    job.PaymentState |= JobPaymentState.RemainReserved;
                    cashOperationService.FreezeUserMoney(job.StudentUserId, job.GetStudentRemainAmount());
                    break;
                case JobActionType.AcceptWithoutRemainAmount:
                    job.Status = JobStatus.Accepted;
                    cashOperationService.UnfreezeUserMoney(job.TeacherUserId, job.TeacherPrepaymentAmount);
                    break;
                case JobActionType.Accept:
                    job.Status = JobStatus.Accepted;
                    cashOperationService.UnfreezeUserMoney(job.TeacherUserId, job.TeacherPrepaymentAmount);
                    if (job.PaymentState.HasFlag(JobPaymentState.RemainReserved) && job.GetStudentRemainAmount() > 0)
                    {
                        var transactionDescription = $"Оплата выполненной работы {job.Title}";
                        cashOperationService.TransferMoneyFromUserToUser(job.StudentUserId, job.TeacherUserId, job.GetStudentRemainAmount(), job.GetRemainCommission(), TransactionType.CompleteJobPayment, transactionDescription);
                        cashOperationService.UnfreezeUserMoney(job.StudentUserId, job.GetStudentRemainAmount());
                    }
                    break;
                case JobActionType.Cancel:
                    job.Status = JobStatus.Cancelled;
                    cashOperationService.UnfreezeUserMoney(job.StudentUserId, job.StudentPrepaymentAmount);
                    job.StudentPrepaymentAmount = 0;
                    break;
                case JobActionType.Reject:
                    job.Status = JobStatus.InReWorking;
                    break;
                case JobActionType.OfferAbort:
                    job.Status = JobStatus.AbortOffered;
                    break;
                case JobActionType.ConfirmAbort:
                    job.Status = JobStatus.Aborted;
                    var revertPrepaymentTransactionDescription = $"Предоплата по задаче {job.Title}";
                    cashOperationService.RevertTransferMoneyFromUserToUser(job.StudentUserId, job.TeacherUserId, job.StudentPrepaymentAmount, job.PrepaymentCommission, TransactionType.JobPrepayment, revertPrepaymentTransactionDescription);
                    cashOperationService.UnfreezeUserMoney(job.TeacherUserId, job.TeacherPrepaymentAmount);
                    if (job.PaymentState.HasFlag(JobPaymentState.RemainReserved) && job.GetStudentRemainAmount() > 0)
                    {
                        job.PaymentState ^= JobPaymentState.RemainReserved;
                        cashOperationService.UnfreezeUserMoney(job.StudentUserId, job.GetStudentRemainAmount());
                    }
                    break;
                case JobActionType.RejectAbort:
                    job.Status = JobStatus.InArbitrage;
                    break;
                case JobActionType.Edit:
                    return job;
                case JobActionType.Delete:
                    foreach (var attachment in job.Attachments)
                    {
                        uploadedFileRepository.Delete(attachment.FileName);
                    }
                    jobRepository.Remove(job.Id);
                    return null;
                default:
                    throw new NotImplementedException($"Неизвестное действие {actionType}");
            }

            jobRepository.Write(job);

            return job;
        }

        private class JobActionByUserRole
        {
            public JobActionByUserRole(JobActionType actionType, UserRole userRole, ISpecification<Job>[] specifications = null)
                : this(actionType, new[] { userRole }, specifications)
            {
            }

            public JobActionByUserRole(JobActionType actionType, UserRole[] userRoles, ISpecification<Job>[] specifications = null)
            {
                ActionType = actionType;
                UserRoles = userRoles;
                Specifications = specifications ?? new ISpecification<Job>[0];
            }

            public JobActionType ActionType { get; }
            public UserRole[] UserRoles { get; }
            public ISpecification<Job>[] Specifications { get; }

            public bool IsUserHasAccess(ApplicationUser user, Job job)
            {
                var filteredUserRoles = UserRoles.Select(y => y.Name).Intersect(user.Roles).ToArray();
                return filteredUserRoles.Contains(UserRole.Admin.Name) ||
                    (filteredUserRoles.Contains(UserRole.Student.Name) && job.StudentUserId == user.Id && job.TeacherUserId != user.Id) ||
                    (filteredUserRoles.Contains(UserRole.Teacher.Name) && (string.IsNullOrEmpty(job.TeacherUserId) || job.TeacherUserId == user.Id) && job.StudentUserId != user.Id);
            }
        }
    }
}