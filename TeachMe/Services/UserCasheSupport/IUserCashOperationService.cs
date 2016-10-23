using TeachMe.Models.Transactions;

namespace TeachMe.Services.UserCasheSupport
{
    public interface IUserCashOperationService
    {
        void TransferMoneyFromUserToUser(string sourceUserId, string recipientUserId, double amount, double commissionAmount, TransactionType transactionType, string transactionDescription);
        void AddMoneyToUser(string userId, double amount);
        void SubtractMoneyFromUser(string userId, double amount);
        void FreezeUserMoney(string userId, double amount);
        void UnfreezeUserMoney(string userId, double amount);
    }
}