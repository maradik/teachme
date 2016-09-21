namespace TeachMe.Services.Notifications
{
    public interface ICustomSmsService
    {
        int Priority { get; }
        void Send(string recipientPhone, string text);
    }
}