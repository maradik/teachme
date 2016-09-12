using System.Linq;
using MongoDB.Driver.Builders;
using TeachMe.Models.Payments;

namespace TeachMe.DataAccess.Payments
{
    public class InvoiceRepository : RepositoryBase<Invoice, int>, IInvoiceRepository
    {
        public InvoiceRepository(InvoiceRepositoryParameters parameters) : base(parameters)
        {
        }

        protected override int CreateNewId()
        {
            return GetMaxId() + 1;
        }

        private int GetMaxId()
        {
            return Collection.FindAll().SetSortOrder(SortBy<Invoice>.Descending(x => x.Id)).SetLimit(1).FirstOrDefault()?.Id ?? 0;
        }
    }
}