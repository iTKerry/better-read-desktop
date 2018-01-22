using GalaSoft.MvvmLight.Messaging;

namespace LoveRead.Infrastructure.Services
{
    public class LogMessange
    {
        public string Text { get; set; }
    }

    public class MessangerService : IMessangerService
    {
        public void Log(LogMessange content) =>
            Messenger.Default.Send(new NotificationMessage<LogMessange>(this, content, "Sending Log messange"));
    }

    public interface IMessangerService
    {
        void Log(LogMessange content);
    }
}