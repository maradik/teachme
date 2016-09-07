namespace TeachMe.Services.UserCash
{
    public interface IUserCashOperationService
    {
        void TransferMoneyFromUserToUser(string sourceUserId, string recipientUserId, double sourceAmount, double recipientAmount, UserCashTransferType transferType);
        void FreezeUserMoney(string userId, double amount);
        void UnfreezeUserMoney(string userId, double amount);
    }
}