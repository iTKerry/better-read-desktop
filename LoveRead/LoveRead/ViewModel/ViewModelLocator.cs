using System.Text;
using GalaSoft.MvvmLight.Ioc;
using LoveRead.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using ScrapySharp.Network;

namespace LoveRead.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var browser = new ScrapingBrowser{Encoding = Encoding.GetEncoding("windows-1251")};

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ILibraryScrapper>(() => new LibraryScrapper(browser));
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}