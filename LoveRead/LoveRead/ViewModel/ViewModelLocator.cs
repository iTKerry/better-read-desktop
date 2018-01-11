using GalaSoft.MvvmLight.Ioc;
using LoveRead.Services;
using Microsoft.Practices.ServiceLocation;
using ScrapySharp.Network;

namespace LoveRead.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var browser = new ScrapingBrowser();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IScrapper>(() => new Scrapper(browser));
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}