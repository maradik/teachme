using System;
using System.Linq.Expressions;
using TeachMe.Models.Payouts;

namespace TeachMe.DataAccess.Payouts
{
    public interface IPayoutRepository
    {
        void Write(Payout model);
        Payout Get(Guid id);
        void Remove(Guid id);
        Payout[] Get(Expression<Func<Payout, bool>> whereExpression = null, Expression<Func<Payout, object>> orderByExpression = null, SortOrder sortOrder = SortOrder.Ascending, int? limit = null);
        Payout GetLastFor(string userId);
    }
}