using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.Properties;
using LoveRead.ViewModel;

namespace LoveRead.Views.Tabs.SaveBook
{
    public class SaveBookViewModel : BaseViewModel
    {
        private readonly IDocService _docService;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start(SaveAsPath));

        public SaveBookViewModel(IDocService docService)
        {
            _docService = docService;
            Messenger.Default.Register<NotificationMessage<TabSwitchMessange>>(this, ProcessTabSwitchMessange);

            Start();
        }

        protected override Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(Settings.Default.DownloadPath))
                Settings.Default.DownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SaveAsPath = Settings.Default.DownloadPath;

            return base.InitializeAsync();
        }

        public override void Dispose()
            => Settings.Default.DownloadPath = SaveAsPath;

        public SaveBookView SaveBookView { get; set; }

        private WebBook _book;
        private WebBook Book
        {
            get => _book;
            set => Set(() => Book, ref _book, value);
        }

        private string _saveAsPath;
        public string SaveAsPath
        {
            get => _saveAsPath;
            set => Set(() => SaveAsPath, ref _saveAsPath, value);
        }

        private void ProcessTabSwitchMessange(NotificationMessage<TabSwitchMessange> message)
        {
            if (!Equals(GetType().Name, message.Target))
                return;

            var book = (WebBook)message.Content.Data;
            if (Book is null || Book.Id != book.Id)
                Book = book;
        }

        private void GetSaveAsPath()
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    SaveAsPath = dialog.SelectedPath;
        }
    }
}
