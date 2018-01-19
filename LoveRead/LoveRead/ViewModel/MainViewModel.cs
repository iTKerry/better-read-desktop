using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure;
using LoveRead.Model;
using LoveRead.Views;

namespace LoveRead.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILibraryScrapper _libraryScrapper;
        private readonly IDocService _docService;

        public IMainView MainView;

        public RelayCommand ReadBookCommand => new RelayCommand(async () =>
        {
            Book = await _libraryScrapper.ReadBook(BookUrl);
            BookLogo = Book.ImageUrl;
        });

        public RelayCommand GenerateDocCommand => new RelayCommand(() 
            => _docService.Save(Book));

        public MainViewModel(ILibraryScrapper libraryScrapper, IDocService docService)
        {
            _libraryScrapper = libraryScrapper;
            _docService = docService;
            BookLogo = string.Empty;
#if DEBUG
            BookUrl = "http://loveread.ec/read_book.php?id=69223&p=1";
#endif
            LogList = new ObservableCollection<string>();
            Messenger.Default.Register<NotificationMessage<LogMessange>>(this, ProcessLogMessage);
        }

        private void ProcessLogMessage(NotificationMessage<LogMessange> messange)
        {
            LogList.Add(messange.Content.Text);
            MainView.ScrollLogToEnd();
        }

        private WebBook Book { get; set; }

        private bool _isReadButtonEnabled;
        public bool IsReadButtonEnabled
        {
            get => _isReadButtonEnabled;
            set => Set(() => IsReadButtonEnabled, ref _isReadButtonEnabled, value);
        }

        private bool _isGenerateButtonEnabled;
        public bool IsGenerateButtonEnabled
        {
            get => _isReadButtonEnabled;
            set => Set(() => IsGenerateButtonEnabled, ref _isGenerateButtonEnabled, value);
        }

        private string _bookUrl;
        public string BookUrl
        {
            get => _bookUrl;
            set
            {
                IsReadButtonEnabled = !string.IsNullOrEmpty(value);
                Set(() => BookUrl, ref _bookUrl, value);
            }
        }

        private string _bookLogo;
        public string BookLogo
        {
            get => _bookLogo;
            set => Set(() => BookLogo, ref _bookLogo, value);
        }

        private ObservableCollection<string> _logList;
        public ObservableCollection<string> LogList
        {
            get => _logList;
            set => Set(() => LogList, ref _logList, value);
        }
    }
}