using GalaSoft.MvvmLight.Messaging;

namespace LoveRead.Infrastructure.Services
{
    public class LogMessange
    {
        public string Text { get; set; }
    }

    public class ProgressMessange
    {
        public int Current { get; set; }
        public int Total { get; set; }
        public ProgressType ProgressType { get; set; }
    }

    public enum ProgressType
    {
        Reading,
        Writing
    }

    public class MessangerService : IMessangerService
    {
        public void NotifyLog(LogMessange content) =>
            Messenger.Default.Send(new NotificationMessage<LogMessange>(this, content, "Sending NotifyLog messange"));

        public void NotifyProgress(ProgressMessange content) =>
            Messenger.Default.Send(new NotificationMessage<ProgressMessange>(this, content, "Sending Progress messange"));
    }

    public interface IMessangerService
    {
        void NotifyLog(LogMessange content);
        void NotifyProgress(ProgressMessange content);
    }
}