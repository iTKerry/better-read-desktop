using GalaSoft.MvvmLight;
using LoveRead.Infrastructure;

namespace LoveRead.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILibraryScrapper _libraryScrapper;

        public MainViewModel(ILibraryScrapper libraryScrapper)
        {
            _libraryScrapper = libraryScrapper;
            _libraryScrapper.ReadBook("http://loveread.ec/read_book.php?id=69223&p=1");
        }
    }
}