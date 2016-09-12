namespace TeachMe.Models.Payments
{
    public class Invoice : IEntity<int>
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public InvoiceStatus Status { get; set; }
        public long CreationTicks { get; set; }
    }
}