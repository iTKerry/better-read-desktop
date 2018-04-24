using System.ComponentModel;
using LoveRead.Properties;

namespace LoveRead.Views.Main
{
    public partial class MainView : IMainView
    {
        private MainViewModel MainViewModel => (MainViewModel) DataContext;

        public MainView()
        {
            InitializeComponent();
            MainViewModel.MainView = this;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Settings.Default.Save();
        }

        public void ScrollLogToEnd() 
            => LogList.ScrollToEnd();
    }

    public interface IMainView
    {
        void ScrollLogToEnd();
    }
}
