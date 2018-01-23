using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.Properties;
using MaterialDesignColors;

namespace LoveRead.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly ILibraryScrapper _libraryScrapper;
        private readonly IDocService _docService;

        public RelayCommand ReadBookCommand
            => new RelayCommand(async () => await ReadBook());

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand 
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand<bool> ToggleBaseCommand
            => new RelayCommand<bool>(ApplyBase);

        public RelayCommand<Swatch> ApplyPrimaryCommand
            => new RelayCommand<Swatch>(ApplyPrimary);

        public RelayCommand<Swatch> ApplyAccentCommand
            => new RelayCommand<Swatch>(ApplyAccent);

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start($@"{SaveAsPath}"));

        public MainViewModel(ILibraryScrapper libraryScrapper, IDocService docService)
        {
            _libraryScrapper = libraryScrapper;
            _docService = docService;

            Swatches = new SwatchesProvider().Swatches;

            Messenger.Default.Register<NotificationMessage<LogMessange>>(this, ProcessLogMessage);
            Messenger.Default.Register<NotificationMessage<ProgressMessange>>(this, ProcessProgressMessage);

            InitData();
        }

        private void InitData()
        {
            if (string.IsNullOrEmpty(Settings.Default.DownloadPath))
                Settings.Default.DownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SaveAsPath = Settings.Default.DownloadPath;

            LogList = new ObservableCollection<string> {"Application started!\n...\n"};
            BookLogo = string.Empty;
            BookPagesCount = 0;
            IsBookUrlEnabled = true;
            BookName = "Search some book!";
            BookAuthor = "No book, no author";
#if DEBUG
            BookUrl = "http://loveread.ec/read_book.php?id=14458&p=1";
#endif
        }

        private void ProcessLogMessage(NotificationMessage<LogMessange> messange)
        {
            LogList.Add(messange.Content.Text);
            MainView.ScrollLogToEnd();
        }

        private async Task ReadBook()
        {
            Book = await _libraryScrapper.ReadBook(BookUrl);
            BookLogo = Book.ImageUrl;
            BookName = Book.Name;
            BookAuthor = Book.Author;
            BookPagesCount = Book.PagesCount;
        }

        private void GetSaveAsPath()
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    SaveAsPath = dialog.SelectedPath;
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