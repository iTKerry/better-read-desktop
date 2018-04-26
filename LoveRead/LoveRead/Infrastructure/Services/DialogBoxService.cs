using System;
using System.Windows.Forms;

namespace LoveRead.Infrastructure.Services
{
    public class DialogBoxService : IDialogBoxService
    {
        public DialogResult ShowMessage(string message, string title)
            => MessageBox.Show(message, title, MessageBoxButtons.YesNo);

        public void ShowFolderBrowserDialog(Action<bool, string> afterHideCallback)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    afterHideCallback(false, string.Empty);
                    return;
                }
                afterHideCallback(true, dialog.SelectedPath);
            }
        }
    }

    public interface IDialogBoxService
    {
        DialogResult ShowMessage(string message, string title);
        void ShowFolderBrowserDialog(Action<bool, string> afterHideCallback);
    }
}
