using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.Views;

namespace LoveRead.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ILibraryScrapper _libraryScrapper;
        private readonly IDocService _docService;

        public IMainView MainView;

        public RelayCommand ReadBookCommand => new RelayCommand(async () =>
        {
            Book = await _libraryScrapper.ReadBook(BookUrl);
            BookLogo = Book.ImageUrl;
            BookName = Book.Name;
        });

        public RelayCommand GenerateDocCommand => new RelayCommand(() 
            => _docService.Save(Book));

        public MainViewModel(ILibraryScrapper libraryScrapper, IDocService docService)
        {
            _libraryScrapper = libraryScrapper;
            _docService = docService;

            Messenger.Default.Register<NotificationMessage<LogMessange>>(this, ProcessLogMessage);
            Messenger.Default.Register<NotificationMessage<ProgressMessange>>(this, ProcessProgressMessage);

            InitData();
        }

        private void InitData()
        {
            LogList = new ObservableCollection<string> {"Application started!\n...\n"};
            BookLogo = string.Empty;
            BookName = "Search some book!";
#if DEBUG
            BookUrl = "http://loveread.ec/read_book.php?id=14458&p=1";
#endif
        }

        private void ProcessLogMessage(NotificationMessage<LogMessange> messange)
        {
            LogList.Add(messange.Content.Text);
            MainView.ScrollLogToEnd();
        }

        private void ProcessProgressMessage(NotificationMessage<ProgressMessange> message)
        {
            ReadingProgress = (int)((double)message.Content.Current / (double)message.Content.Total * 100);
            switch (ReadingProgress)
            {
                case 1:
                    IsReading = true;
                    IsReadingComplete = false;
                    break;
                case 100:
                    IsReading = false;
                    IsReadingComplete = true;
                    break;
            }
        }
    }
}