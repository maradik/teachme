using System;
using System.Linq;
using TeachMe.DataAccess.Payouts;
using TeachMe.DataAccess.Transactions;
using TeachMe.Extensions;
using TeachMe.Helpers.Settings;
using TeachMe.Models.Payouts;
using TeachMe.Models.Transactions;
using TeachMe.Models.Users;
using TeachMe.Services.Notifications;
using TeachMe.Services.UserCasheSupport;

namespace TeachMe.Services.Payouts
{
    public class PayoutActionService : IPayoutActionService
    {
        private readonly IPayoutRepository payoutRepository;
        private readonly IUserCashOperationService userCashOperationService;
        private readonly ITransactionRepository transactionRepository;
        private readonly ISmsService smsService;
        private readonly ApplicationUserManager applicationUserManager;

        public PayoutActionService(IPayoutRepository payoutRepository,
                                   IUserCashOperationService userCashOperationService,
                                   ITransactionRepository transactionRepository,
                                   ISmsService smsService,
                                   ApplicationUserManager applicationUserManager)
        {
            this.payoutRepository = payoutRepository;
            this.userCashOperationService = userCashOperationService;
            this.transactionRepository = transactionRepository;
            this.smsService = smsService;
            this.applicationUserManager = applicationUserManager;
        }

        public void CreatePayout(double amount, string qiwiPhoneNumber, ApplicationUser user)
        {
            var availableAmount = user.Cash.AvailableAmount;
            if (amount > availableAmount)
            {
                throw new InvalidPayoutAmountException($"Сумма не может превышать {availableAmount} руб.");
            }

            var payout = new Payout
            {
                UserId = user.Id,
                Status = PayoutStatus.Pending,
                Amount = amount,
                Recipient = new PayoutRecipient {QiwiPhoneNumber = qiwiPhoneNumber}
            };

            payoutRepository.Write(payout);
            userCashOperationService.FreezeUserMoney(user.Id, payout.Amount);
            SendSmsToAdmin("Новая заявка на выплату.");
        }

        public void PerformPayout(Guid id)
        {
            var payout = payoutRepository.Get(id);
            if (payout.Status != PayoutStatus.Pending)
            {
                throw new PayoutActionException($"Невозможно выполнить выплату, т.к. статус {payout.Status}");
            }
            var payoutUser = payout.GetUser();
            if (payoutUser.Cash.PhysicalAmount < payout.Amount)
            {
                throw new PayoutActionException($"Невозможно выполнить выплату, т.к. у пользователя недостаточно средств");
            }

            payout.Status = PayoutStatus.Done;
            payoutRepository.Write(payout);
            userCashOperationService.UnfreezeUserMoney(payout.UserId, payout.Amount);
            userCashOperationService.SubtractMoneyFromUser(payout.UserId, payout.Amount);
            WriteTransactionFor(payout);
            SendSmsToUser(payoutUser, $"Выплата в размере {(int)payout.Amount}руб. произведена.");
        }

        public void DiscardPayout(Guid id, string admincomment)
        {
            var payout = payoutRepository.Get(id);
            if (payout.Status != PayoutStatus.Pending)
            {
                throw new PayoutActionException($"Невозможно отменить выплату, т.к. статус {payout.Status}");
            }
            payout.Status = PayoutStatus.Discarded;
            payout.AdminComment = admincomment;
            payoutRepository.Write(payout);
            userCashOperationService.UnfreezeUserMoney(payout.UserId, payout.Amount);
        }

        private void WriteTransactionFor(Payout payout)
        {
            var transaction = TransactionBuilder.With(TransactionType.Payout)
                                                .SetText($"Выплата денежных средств пользователю {payout.GetUser().UserName} в размере {payout.Amount}. Заявка на выплату {payout.Id}")
                                                .AddPart(TransactionPartType.Debit, payout.Amount, new TransactionAccount { PayoutId = payout.Id })
                                                .AddPart(TransactionPartType.Credit, payout.Amount, new TransactionAccount { UserId = payout.UserId })
                                                .Build();

            transactionRepository.Write(transaction);
        }

        private void SendSmsToAdmin(string text)
        {
            var admins = applicationUserManager.Users.Where(x => x.Roles.Contains(UserRole.Names.Admin)).ToArray();
            foreach (var admin in admins)
            {
                SendSmsToUser(admin, text);
            }
        }

        private void SendSmsToUser(ApplicationUser user, string text)
        {
            if (string.IsNullOrEmpty(user.PhoneNumber))
                return;
            var signature = user.Roles.Contains(UserRole.Names.Student)
                                ? ApplicationSettings.StudentProjectName
                                : ApplicationSettings.TeacherProjectName;
            smsService.Send(user.PhoneNumber, text + $" {signature}");
        }
    }
}