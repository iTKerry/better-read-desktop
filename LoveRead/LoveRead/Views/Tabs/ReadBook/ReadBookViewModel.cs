using System;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.ViewModel;
using LoveRead.Views.Tabs.BookDetails;
using MaterialDesignThemes.Wpf.Transitions;

namespace LoveRead.Views.Tabs.ReadBook
{
    public partial class ReadBookViewModel : BaseViewModel
    {
        private readonly ILibraryScrapper _libraryScrapper;
        private readonly IMessangerService _messanger;

        public RelayCommand ReadBookCommand
            => new RelayCommand(async () => await ReadBook());

        public RelayCommand MoveNextCommand
            => new RelayCommand(MoveNext);

        public ReadBookViewModel(ILibraryScrapper libraryScrapper, IMessangerService messanger)
        {
            _libraryScrapper = libraryScrapper;
            _messanger = messanger;

            Messenger.Default.Register<NotificationMessage<TabSwitchMessange>>(this, ProcessTabSwitchMessange);
            Messenger.Default.Register<NotificationMessage<ProgressMessange>>(this, ProcessProgressMessage);

            Start();
        }
        
        protected override Task InitializeAsync()
        {
            IsMoveNextVisible = false;
#if DEBUG
            BookUrl = "http://loveread.ec/read_book.php?id=14458&p=1";
#endif
            return base.InitializeAsync();
        }

        private async Task ReadBook()
        {
            IsMoveNextVisible = false;
            Book = await _libraryScrapper.ReadBook(BookUrl);
            new Thread(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                DispatcherHelper.CheckBeginInvokeOnUI(() => MoveNextCommand.Execute(null));
            }).Start();
        }

        private void MoveNext()
        {
            _messanger.NotifyTabSwitch(this, nameof(BookDetailsViewModel), new TabSwitchMessange {Data = Book});
            Transitioner.MoveNextCommand.Execute(null, ReadBookView);
        }

        private void ProcessTabSwitchMessange(NotificationMessage<TabSwitchMessange> message)
        {
            if (!Equals(GetType().Name, message.Target))
                return;

            IsReadButtonEnabled = true;
            IsBookUrlReadOnly = false;
            IsMoveNextVisible = true;
        }

        private void ProcessProgressMessage(NotificationMessage<ProgressMessange> message)
        {
            ReadingProgress = (int)((double)message.Content.Current / (double)message.Content.Total * 100);
            switch (ReadingProgress)
            {
                case 1:
                    IsReading = true;
                    IsReadButtonEnabled = false;
                    IsBookUrlReadOnly = true;
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
