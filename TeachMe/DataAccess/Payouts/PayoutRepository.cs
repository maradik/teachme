using System;
using System.Linq;
using TeachMe.Models.Payouts;

namespace TeachMe.DataAccess.Payouts
{
    public class PayoutRepository : RepositoryBase<Payout, Guid>, IPayoutRepository
    {
        public PayoutRepository(PayoutRepositoryParameters parameters) : base(parameters)
        {
        }

        public Payout GetLastFor(string userId)
        {
            return Get(x => x.UserId == userId, x => x.CreationTicks, SortOrder.Descending).FirstOrDefault();
        }

        protected override Guid CreateNewId()
        {
            return Guid.NewGuid();
        }
    }
}