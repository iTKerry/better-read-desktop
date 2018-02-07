using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LoveRead.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        protected async Task ReloadDataAsync()
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        protected virtual Task InitializeAsync() 
            => Task.FromResult(0);

        public async void Start()
            => await ReloadDataAsync();
    }
}
