using System;
using System.Diagnostics;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using LoveRead.Infrastructure.Services;
using LoveRead.Properties;
using LoveRead.ViewModel;

namespace LoveRead.Views.Tabs.BookDetails
{
    public partial class BookDetailsViewModel : BaseViewModel
    {
        private readonly IDocService _docService;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start($@"{SaveAsPath}"));

        public BookDetailsViewModel(IDocService docService)
        {
            _docService = docService;

            if (string.IsNullOrEmpty(Settings.Default.DownloadPath))
                Settings.Default.DownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SaveAsPath = Settings.Default.DownloadPath;
        }

        public override void Dispose()
        {
            Settings.Default.DownloadPath = SaveAsPath;

            base.Dispose();
        }

        private void GetSaveAsPath()
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    SaveAsPath = dialog.SelectedPath;
        }
    }
}
