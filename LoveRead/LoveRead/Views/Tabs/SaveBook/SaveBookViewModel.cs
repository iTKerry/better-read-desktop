using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.Properties;
using LoveRead.ViewModel;
using LoveRead.Views.Tabs.BookDetails;
using MaterialDesignThemes.Wpf.Transitions;

namespace LoveRead.Views.Tabs.SaveBook
{
    public partial class SaveBookViewModel : BaseViewModel
    {
        private readonly IMessangerService _messanger;
        private readonly IDocService _docService;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start(SaveAsPath));

        public RelayCommand MoveBackCommand
            => new RelayCommand(MoveBack);

        public SaveBookViewModel(IDocService docService, IMessangerService messanger)
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

        private async void ProcessTabSwitchMessange(NotificationMessage<TabSwitchMessange> message)
        {
            if (!Equals(GetType().Name, message.Target))
                return;

            var book = (WebBook)message.Content.Data;
            if (Book is null || Book.Id != book.Id)
                Book = book;

            await ReloadDataAsync();
        }

        private void GetSaveAsPath()
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    SaveAsPath = dialog.SelectedPath;
        }

        private void MoveBack()
        {
            _messanger.NotifyTabSwitch(this, nameof(BookDetailsViewModel), new TabSwitchMessange { Data = Book });
            Transitioner.MovePreviousCommand.Execute(null, SaveBookView as IInputElement);
        }

        public override void Dispose()
            => Settings.Default.DownloadPath = SaveAsPath;
    }
}
