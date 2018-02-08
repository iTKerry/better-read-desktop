using LoveRead.Model;

namespace LoveRead.Views.Tabs.ReadBook
{
    public partial class ReadBookViewModel
    {
        public ReadBookView ReadBookView { get; set; }

        private bool _isBookUrlReadOnly;
        public bool IsBookUrlReadOnly
        {
            get => _isBookUrlReadOnly;
            set => Set(() => IsBookUrlReadOnly, ref _isBookUrlReadOnly, value);
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
    }
}