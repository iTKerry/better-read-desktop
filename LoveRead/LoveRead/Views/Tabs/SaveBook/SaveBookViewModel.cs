using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LoveRead.Infrastructure.Services;
using LoveRead.Model;
using LoveRead.Properties;
using LoveRead.ViewModel;

namespace LoveRead.Views.Tabs.SaveBook
{
    public class SaveBookViewModel : BaseViewModel
    {
        private readonly IDocService _docService;
        private readonly object _dummyNode = null;

        public RelayCommand GetSaveAsPathCommand
            => new RelayCommand(GetSaveAsPath);

        public RelayCommand GenerateDocCommand
            => new RelayCommand(() => _docService.SaveAs(Book, SaveAsPath));

        public RelayCommand OpenSaveAsFolderCommand
            => new RelayCommand(() => Process.Start(SaveAsPath));

        public SaveBookViewModel(IDocService docService)
        {
            _docService = docService;
            Messenger.Default.Register<NotificationMessage<TabSwitchMessange>>(this, ProcessTabSwitchMessange);

            Start();
        }

        protected override Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(Settings.Default.DownloadPath))
                Settings.Default.DownloadPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            SaveAsPath = Settings.Default.DownloadPath;
            TreeViewItems = new ObservableCollection<TreeViewItem>();

            foreach (var s in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem
                {
                    Header = s,
                    Tag = s,
                    FontWeight = FontWeights.Normal
                };
                item.Items.Add(_dummyNode);
                item.Expanded += FolderExpanded;
                TreeViewItems.Add(item);
            }

            ExpandTree(SaveAsPath);

            return base.InitializeAsync();
        }

        public override void Dispose()
            => Settings.Default.DownloadPath = SaveAsPath;

        public SaveBookView SaveBookView { get; set; }

        private ObservableCollection<TreeViewItem> _treeViewItems;
        public ObservableCollection<TreeViewItem> TreeViewItems
        {
            get => _treeViewItems;
            set => Set(() => TreeViewItems, ref _treeViewItems, value);
        }

        private WebBook _book;
        private WebBook Book
        {
            get => _book;
            set => Set(() => Book, ref _book, value);
        }

        private string _saveAsPath;
        public string SaveAsPath
        {
            get => _saveAsPath;
            set => Set(() => SaveAsPath, ref _saveAsPath, value);
        }

        private async void ProcessTabSwitchMessange(NotificationMessage<TabSwitchMessange> message)
        {
            if (!Equals(GetType().Name, message.Target))
                return;

            var book = (WebBook)message.Content.Data;
            if (Book is null || Book.Id != book.Id)
                Book = book;

            await ReloadDataAsync();
        }

        private void ExpandTree(string path)
        {
            TreeViewItem selectedFolder = null;
            var folders = path.Split('\\');
            for (var index = 0; index < folders.Length; index++)
            {
                if (index == 0)
                    selectedFolder = TreeViewItems
                        .First(item => ((string) item.Header).Contains(folders[index]));

                FolderExpanded(selectedFolder, null);

                if (index != 0)
                    selectedFolder = selectedFolder?.Items.OfType<TreeViewItem>()
                        .First(item => ((string) item.Header).Contains(folders[index]));

                if (index + 1 == folders.Length)
                    if (selectedFolder != null)
                        selectedFolder.IsSelected = true;
            }
        }

        private void FolderExpanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == _dummyNode)
            {
                item.Items.Clear();
                item.IsExpanded = true;
                foreach (var s in Directory.GetDirectories(item.Tag.ToString()))
                {
                    var dirInfo = new DirectoryInfo(s);
                    if ((dirInfo.Attributes & FileAttributes.Hidden) != 0 ||
                        (dirInfo.Attributes & FileAttributes.System) != 0)
                        continue;
                    var subitem = new TreeViewItem
                    {
                        Header = s.Substring(s.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                        Tag = s,
                        FontWeight = FontWeights.Normal
                    };
                    subitem.Items.Add(_dummyNode);
                    subitem.Expanded += FolderExpanded;
                    item.Items.Add(subitem);
                }
            }
        }

        private void GetSaveAsPath()
        {
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    SaveAsPath = dialog.SelectedPath;
        }
    }
}
