using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace LoveRead.ViewModel
{
    public class BaseViewModel : ViewModelBase, IDisposable
    {
        public async void Start()
            => await ReloadDataAsync();

        protected async Task ReloadDataAsync()
        {
            try
            {
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        protected virtual Task InitializeAsync() 
            => Task.FromResult(0);

        public virtual void Dispose()
        {
        }
    }
}
