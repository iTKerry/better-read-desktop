using GalaSoft.MvvmLight;
using LoveRead.Services;

namespace LoveRead.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IScrapper _scrapper;

        public MainViewModel(IScrapper scrapper)
        {
            _scrapper = scrapper;
        }
    }
}