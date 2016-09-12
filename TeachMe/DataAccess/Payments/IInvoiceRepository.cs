using System;
using TeachMe.Models.Payments;

namespace TeachMe.DataAccess.Payments
{
    public interface IInvoiceRepository
    {
        void Write(Invoice model);
        Invoice Get(Int32 id);
        void Remove(Int32 id);
    }
}