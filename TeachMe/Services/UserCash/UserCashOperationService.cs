using System;
using Microsoft.AspNet.Identity;

namespace TeachMe.Services.UserCash
{
    public class UserCashOperationService : IUserCashOperationService
    {
        private readonly ApplicationUserManager applicationUserManager;

        public UserCashOperationService(ApplicationUserManager applicationUserManager)
        {
            this.applicationUserManager = applicationUserManager;
        }

        public void TransferMoneyFromUserToUser(string sourceUserId, string recipientUserId, double sourceAmount, double recipientAmount, UserCashTransferType transferType)
        {
            if (0 >= sourceAmount)
                throw new ArgumentOutOfRangeException(nameof(sourceAmount), sourceAmount, "Значение должно быть больше 0");

            if (0 >= recipientAmount)
                throw new ArgumentOutOfRangeException(nameof(recipientAmount), recipientAmount, "Значение должно быть больше 0");

            AddAmountToUserCash(sourceUserId, -sourceAmount, transferType.Source, (cash, newAmount) => newAmount >= 0);
            AddAmountToUserCash(recipientUserId, recipientAmount, transferType.Recipient);
        }

        public void FreezeUserMoney(string userId, double amount)
        {
            if (0 >= amount)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Значение должно быть больше 0");

            AddAmountToUserCash(userId, amount, UserCashMemberType.Frozen, (cash, newAmount) => cash.PhysicalAmount >= newAmount);
        }

        public void UnfreezeUserMoney(string userId, double amount)
        {
            if (0 >= amount)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Значение должно быть больше 0");

            AddAmountToUserCash(userId, -amount, UserCashMemberType.Frozen, (cash, newAmount) => newAmount >= 0);
        }

        private void AddAmountToUserCash(string userId, double amount, UserCashMemberType cashMemberType, Func<Models.Users.UserCash, double, bool> isValidCashNewAmount = null)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            isValidCashNewAmount = isValidCashNewAmount ?? ((c, a) => true);

            var user = applicationUserManager.FindById(userId);
            var originAmount = GetCashMemberValue(user.Cash, cashMemberType);
            var newAmount = originAmount + amount;

            if (!isValidCashNewAmount(user.Cash, newAmount))
                throw new InvalidOperationException($"Некорректная операция по изменению {cashMemberType} пользователя {userId} на сумму {amount}, при первоначальном балансе {originAmount}");

            SetCashMemberValue(user.Cash, cashMemberType, newAmount);
            applicationUserManager.Update(user);
        }


        private double GetCashMemberValue(Models.Users.UserCash cash, UserCashMemberType memberType)
        {
            switch (memberType)
            {
                case UserCashMemberType.Physical:
                    return cash.PhysicalAmount;
                case UserCashMemberType.Frozen:
                    return cash.FrozenAmount;
                default: 
                    throw new NotImplementedException($"Способ получения значения для {memberType} {nameof(Models.Users.UserCash)} не реализован");
            }
        }

        private void SetCashMemberValue(Models.Users.UserCash cash, UserCashMemberType memberType, double value)
        {
            switch (memberType)
            {
                case UserCashMemberType.Physical:
                    cash.PhysicalAmount = value;
                    break;
                case UserCashMemberType.Frozen:
                    cash.FrozenAmount = value;
                    break;
                default:
                    throw new NotImplementedException($"Способ присвоения значения для {memberType} {nameof(Models.Users.UserCash)} не реализован");
            }
        }
    }
}