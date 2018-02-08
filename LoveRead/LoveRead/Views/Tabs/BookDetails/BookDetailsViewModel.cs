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
using LoveRead.Views.Tabs.ReadBook;

namespace LoveRead.Views.Tabs.BookDetails
{
    public partial class BookDetailsViewModel : BaseViewModel
    {
        private readonly IDocService _docService;
        private readonly IMessangerService _messanger;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start($@"{SaveAsPath}"));

        public RelayCommand MoveBackCommand
            => new RelayCommand(MoveBack);

        public BookDetailsViewModel(IDocService docService, IMessangerService messanger)
        {
            _docService = docService;
            _messanger = messanger;

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

        private void MoveBack()
            => _messanger.NotifyTabSwitch(this, nameof(ReadBookViewModel), new TabSwitchMessange());

        private void ProcessTabSwitchMessange(NotificationMessage<TabSwitchMessange> message)
        {
            if (!Equals(GetType().Name, message.Target))
                return;

            var book = (WebBook) message.Content.Data;
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
