namespace TeachMe.Services.UserCash
{
    public class UserCashTransferType
    {
        public static readonly UserCashTransferType FromPhysicalToFrozen = new UserCashTransferType(UserCashMemberType.Physical, UserCashMemberType.Frozen);
        public static readonly UserCashTransferType FromFrozenToPhysical = new UserCashTransferType(UserCashMemberType.Frozen, UserCashMemberType.Physical);

        private UserCashTransferType(UserCashMemberType source, UserCashMemberType recipient)
        {
            Source = source;
            Recipient = recipient;
        }

        public UserCashMemberType Source { get; }
        public UserCashMemberType Recipient { get; }

        public override string ToString()
        {
            return $"{nameof(Source)}={Source}, {nameof(Recipient)}={Recipient}";
        }
    }
}