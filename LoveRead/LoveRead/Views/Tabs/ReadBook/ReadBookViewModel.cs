using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.ViewModel;

namespace LoveRead.Views.Tabs.ReadBook
{
    public class ReadBookViewModel : BaseViewModel
    {
        private readonly ILibraryScrapper _libraryScrapper;

        public RelayCommand ReadBookCommand
            => new RelayCommand(async () => await ReadBook());

        public ReadBookViewModel(ILibraryScrapper libraryScrapper)
        {
            _libraryScrapper = libraryScrapper;
            Messenger.Default.Register<NotificationMessage<ProgressMessange>>(this, ProcessProgressMessage);
        }

        private bool _isBookUrlEnabled;
        public bool IsBookUrlEnabled
        {
            get => _isBookUrlEnabled;
            set => Set(() => IsBookUrlEnabled, ref _isBookUrlEnabled, value);
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

        private bool _isReadButtonEnabled;
        public bool IsReadButtonEnabled
        {
            get => _isReadButtonEnabled;
            set => Set(() => IsReadButtonEnabled, ref _isReadButtonEnabled, value);
        }

        private bool _isReading;
        public bool IsReading
        {
            get => _isReading;
            set => Set(() => IsReading, ref _isReading, value);
        }

        private bool _isReadingComplete;
        public bool IsReadingComplete
        {
            get => _isReadingComplete;
            set => Set(() => IsReadingComplete, ref _isReadingComplete, value);
        }

        private int _readingProgress;
        public int ReadingProgress
        {
            get => _readingProgress;
            set => Set(() => ReadingProgress, ref _readingProgress, value);
        }

        private WebBook _book;
        private WebBook Book
        {
            get => _book;
            set => Set(() => Book, ref _book, value);
        }

        private async Task ReadBook()
        {
            Book = await _libraryScrapper.ReadBook(BookUrl);
        }

        private void ProcessProgressMessage(NotificationMessage<ProgressMessange> message)
        {
            ReadingProgress = (int)((double)message.Content.Current / (double)message.Content.Total * 100);
            switch (ReadingProgress)
            {
                case 1:
                    IsReading = true;
                    IsReadButtonEnabled = false;
                    IsBookUrlEnabled = false;
                    IsReadingComplete = false;
                    break;
                case 100:
                    IsReading = false;
                    IsReadButtonEnabled = true;
                    IsBookUrlEnabled = true;
                    IsReadingComplete = true;
                    break;
            }
        }
    }
}
