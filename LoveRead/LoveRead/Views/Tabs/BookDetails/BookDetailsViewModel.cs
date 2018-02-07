using System;
using System.Diagnostics;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.Properties;
using LoveRead.ViewModel;

namespace LoveRead.Views.Tabs.BookDetails
{
    public class BookDetailsViewModel : BaseViewModel
    {
        private readonly IDocService _docService;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start($@"{SaveAsPath}"));

        public BookDetailsViewModel(IDocService docService)
        {
            _docService = docService;

            if (string.IsNullOrEmpty(Settings.Default.DownloadPath))
                Settings.Default.DownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SaveAsPath = Settings.Default.DownloadPath;
        }

        private string _saveAsPath;
        public string SaveAsPath
        {
            get => _saveAsPath;
            set => Set(() => SaveAsPath, ref _saveAsPath, value);
        }

        private WebBook _book;
        private WebBook Book
        {
            get => _book;
            set => Set(() => Book, ref _book, value);
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

        private bool _isGenerateButtonEnabled;
        public bool IsGenerateButtonEnabled
        {
            get => _isGenerateButtonEnabled;
            set => Set(() => IsGenerateButtonEnabled, ref _isGenerateButtonEnabled, value);
        }

        private void GetSaveAsPath()
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    SaveAsPath = dialog.SelectedPath;
        }
    }
}
