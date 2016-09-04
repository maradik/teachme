namespace TeachMe.Models.Users
{
    public class UserCash
    {
        public double PhysicalAmount { get; set; }
        public double FrozenAmount { get; set; }
        public double AvailableAmount => PhysicalAmount - FrozenAmount;
    }
}