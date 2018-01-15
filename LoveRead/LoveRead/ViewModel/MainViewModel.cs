using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure;
using LoveRead.Views;

namespace LoveRead.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILibraryScrapper _libraryScrapper;
        public IMainView MainView;

        public RelayCommand ReadBookCommand => new RelayCommand(async () 
            => await _libraryScrapper.ReadBook(BookUrl));

        public MainViewModel(ILibraryScrapper libraryScrapper)
        {
            _libraryScrapper = libraryScrapper;
#if DEBUG
            BookUrl = "http://loveread.ec/read_book.php?id=69223&p=1";
#endif
            LogList = new ObservableCollection<string>();
            Messenger.Default.Register<NotificationMessage<LogMessange>>(this, ProcessLogMessage);
        }

        private void ProcessLogMessage(NotificationMessage<LogMessange> messange)
        {
            LogList.Add(messange.Content.Text);
            MainView.ScrollToEnd();
        }

        private string _bookUrl;
        public string BookUrl
        {
            get => _bookUrl;
            set => Set(() => BookUrl, ref _bookUrl, value);
        }

        private ObservableCollection<string> _logList;
        public ObservableCollection<string> LogList
        {
            get => _logList;
            set =>Set(() => LogList, ref _logList, value);
        }
    }
}