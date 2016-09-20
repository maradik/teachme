namespace TeachMe.Services.Notifications
{
    public interface ISmsService
    {
        void Send(string recipientPhone, string text);
    }
}