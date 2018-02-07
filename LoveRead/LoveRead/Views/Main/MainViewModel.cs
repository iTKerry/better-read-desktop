using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.ViewModel;

namespace LoveRead.Views.Main
{
    public class MainViewModel : BaseViewModel
    {
        public IMainView MainView;

        private ObservableCollection<string> _logList;
        public ObservableCollection<string> LogList
        {
            get => _logList;
            set => Set(() => LogList, ref _logList, value);
        }
        public MainViewModel()
        {
            Messenger.Default.Register<NotificationMessage<LogMessange>>(this, ProcessLogMessage);
            InitData();
        }

        private void InitData()
        {
            LogList = new ObservableCollection<string> {"Application started!\n...\n"};
#if DEBUG
            //BookUrl = "http://loveread.ec/read_book.php?id=14458&p=1";
#endif
        }

        private void ProcessLogMessage(NotificationMessage<LogMessange> messange)
        {
            LogList.Add(messange.Content.Text);
            MainView.ScrollLogToEnd();
        }
    }
}