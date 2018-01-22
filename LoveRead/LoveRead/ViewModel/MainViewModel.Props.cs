using System.Collections.ObjectModel;
using LoveRead.Model;

namespace LoveRead.ViewModel
{
    public partial class MainViewModel
    {
        private WebBook _book;
        private WebBook Book
        {
            get => _book;
            set
            {
                Set(() => Book, ref _book, value);
                if (Book != null)
                    IsGenerateButtonEnabled = true;
            }
        }

        private string _bookName;
        public string BookName
        {
            get => _bookName;
            set => Set(() => BookName, ref _bookName, value);
        }

        private string _bookAuthor;
        public string BookAuthor
        {
            get => _bookAuthor;
            set => Set(() => BookAuthor, ref _bookAuthor, value);
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

        private bool _isBookUrlEnabled;
        public bool IsBookUrlEnabled
        {
            get => _isBookUrlEnabled;
            set => Set(() => IsBookUrlEnabled, ref _isBookUrlEnabled, value);
        }

        private string _bookLogo;
        public string BookLogo
        {
            get => _bookLogo;
            set => Set(() => BookLogo, ref _bookLogo, value);
        }

        private int _bookPagesCount;
        public int BookPagesCount
        {
            get => _bookPagesCount;
            set => Set(() => BookPagesCount, ref _bookPagesCount, value);
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

        private bool _isGenerateButtonEnabled;
        public bool IsGenerateButtonEnabled
        {
            get => _isGenerateButtonEnabled;
            set => Set(() => IsGenerateButtonEnabled, ref _isGenerateButtonEnabled, value);
        }

        private ObservableCollection<string> _logList;
        public ObservableCollection<string> LogList
        {
            get => _logList;
            set => Set(() => LogList, ref _logList, value);
        }
    }
}