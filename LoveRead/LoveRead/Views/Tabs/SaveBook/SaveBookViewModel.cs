using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.Properties;
using LoveRead.ViewModel;
using LoveRead.Views.Tabs.ReadBook;
using MaterialDesignThemes.Wpf.Transitions;

namespace LoveRead.Views.Tabs.SaveBook
{
    public partial class SaveBookViewModel : BaseViewModel
    {
        private readonly IMessangerService _messanger;
        private readonly IDocService _docService;
        private readonly IDialogBoxService _dialogService;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPathAction);

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(OpenSaveAsFolderAction);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(SaveBookAsAction);

        public RelayCommand MoveBackCommand
            => new RelayCommand(MoveBackAction);

        public SaveBookViewModel(IDocService docService, IMessangerService messanger, IDialogBoxService dialogService)
        {
            _docService = docService;
            _messanger = messanger;
            _dialogService = dialogService;
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

        private void MoveBackAction()
        {
            _messanger.NotifyTabSwitch(this, nameof(ReadBookViewModel), new TabSwitchMessange(Book));
            Transitioner.MovePreviousCommand.Execute(null, SaveBookView);
        }

        private void GetSaveAsPathAction()
            => _dialogService.ShowFolderBrowserDialog((isOk, path) => SaveAsPath = isOk ? path : SaveAsPath);

        private void SaveBookAsAction()
            => _docService.SaveAs(Book, SaveAsPath, isOk =>
            {

            });

        private void OpenSaveAsFolderAction()
            => Process.Start(SaveAsPath);

        public override void Dispose()
            => Settings.Default.DownloadPath = SaveAsPath;
    }
}
