using GalaSoft.MvvmLight.Messaging;
using LoveRead.Model;
using LoveRead.ViewModel;

namespace LoveRead.Infrastructure.Services
{
    public class MessangerService : IMessangerService
    {
        public void NotifyTabSwitch(BaseViewModel sender, string targetName, TabSwitchMessange content) 
            => Messenger.Default.Send(new NotificationMessage<TabSwitchMessange>(sender, targetName, content, "Sending TabSwitch messange"));

        public void NotifyLog(LogMessange content) 
            => Messenger.Default.Send(new NotificationMessage<LogMessange>(this, content, "Sending NotifyLog messange"));
    }

    public interface IMessangerService
    {
        void NotifyTabSwitch(BaseViewModel sender, string targetName, TabSwitchMessange content);
        void NotifyLog(LogMessange content);
    }
}