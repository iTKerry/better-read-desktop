using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LoveRead.Properties;

namespace LoveRead.Views.Main
{
    public partial class MainWindow : IMainView
    {
        private readonly object _dummyNode = null;

        private MainViewModel MainViewModel => (MainViewModel) DataContext;

        public MainWindow()
        {
            InitializeComponent();
            MainViewModel.MainView = this;

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
                FoldersItem.Items.Add(item);
            }
            
            var temp = FoldersItem.Items.OfType<TreeViewItem>().First(item => (item.Header as string).Contains('C')).IsExpanded = true;
        }

        private void FolderExpanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (item.Items.Count != 1 || item.Items[0] != _dummyNode)
                return;

            item.Items.Clear();
            try
            {
                foreach (var s in Directory.GetDirectories(item.Tag.ToString()))
                {
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
            catch (Exception)
            {
                // ignored
            }
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
