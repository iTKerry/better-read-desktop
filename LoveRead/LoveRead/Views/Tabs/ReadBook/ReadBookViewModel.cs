using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure;
using LoveRead.Infrastructure.Services;
using LoveRead.ViewModel;
using MaterialDesignThemes.Wpf.Transitions;

namespace LoveRead.Views.Tabs.ReadBook
{
    public partial class ReadBookViewModel : BaseViewModel
    {
        private readonly ILibraryScrapper _libraryScrapper;

        public RelayCommand ReadBookCommand
            => new RelayCommand(async () => await ReadBook());

        public RelayCommand MoveNextCommand
            => new RelayCommand(() => Transitioner.MoveNextCommand.Execute(null, ReadBookView));

        public ReadBookViewModel(ILibraryScrapper libraryScrapper)
        {
            _libraryScrapper = libraryScrapper;
            Messenger.Default.Register<NotificationMessage<ProgressMessange>>(this, ProcessProgressMessage);

            Start();
        }
        
        protected override Task InitializeAsync()
        {
#if DEBUG
            BookUrl = "http://loveread.ec/read_book.php?id=14458&p=1";
#endif
            return base.InitializeAsync();
        }

        private async Task ReadBook()
        {
            Book = await _libraryScrapper.ReadBook(BookUrl);
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
                    IsReadButtonEnabled = true;
                    IsBookUrlReadOnly = false;
                    IsReadingComplete = true;
                    break;
            }
        }
    }
}
