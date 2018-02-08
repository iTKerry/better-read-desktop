using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.ViewModel;

namespace LoveRead.Views.Main
{
    public class MainViewModel : BaseViewModel
    {
        public IMainView MainView { get; set; }

        public MainViewModel()
            => Start();

        protected override Task InitializeAsync()
        {
            Messenger.Default.Register<NotificationMessage<LogMessange>>(this, ProcessLogMessage);
            LogList = new ObservableCollection<string> { "Application started!\n...\n" };

            return base.InitializeAsync();
        }

        private ObservableCollection<string> _logList;
        public ObservableCollection<string> LogList
        {
            get => _logList;
            set => Set(() => LogList, ref _logList, value);
        }

        private void ProcessLogMessage(NotificationMessage<LogMessange> messange)
        {
            LogList.Add(messange.Content.Text);
            MainView.ScrollLogToEnd();
        }
    }
}