using System;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.ViewModel;
using LoveRead.Views.Tabs.SaveBook;
using MaterialDesignThemes.Wpf.Transitions;

namespace LoveRead.Views.Tabs.ReadBook
{
    public partial class ReadBookViewModel : BaseViewModel
    {
        private readonly ILibraryScrapper _libraryScrapper;
        private readonly IMessangerService _messanger;

        public ReadBookViewModel(ILibraryScrapper libraryScrapper, IMessangerService messanger)
        {
            _libraryScrapper = libraryScrapper;
            _messanger = messanger;

            Messenger.Default.Register<NotificationMessage<TabSwitchMessange>>(this, ProcessTabSwitchMessange);

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
            IsReading = true;
            IsReadButtonEnabled = false;
            IsBookUrlReadOnly = true;
            IsReadingComplete = false;

            Book = await _libraryScrapper.ReadBook(BookUrl);

            IsReading = false;
            IsReadingComplete = true;
            await Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                DispatcherHelper.CheckBeginInvokeOnUI(() => MoveNextCommand.Execute(null));
            });
        }

        private void MoveNext()
        {
            _messanger.NotifyTabSwitch(this, nameof(SaveBookViewModel), new TabSwitchMessange(Book));
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
    }
}
