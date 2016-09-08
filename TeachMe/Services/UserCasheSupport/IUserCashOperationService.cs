﻿using TeachMe.Models.Transactions;

namespace TeachMe.Services.UserCasheSupport
{
    public interface IUserCashOperationService
    {
        void TransferMoneyFromUserToUser(string sourceUserId, string recipientUserId, double amount, double commissionAmount, TransactionType transactionType, string transactionDescription);
        void FreezeUserMoney(string userId, double amount);
        void UnfreezeUserMoney(string userId, double amount);
    }
}