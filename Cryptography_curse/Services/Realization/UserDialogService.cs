using Cryptography_curse.Services.Interfaces;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Cryptography_curse.Services.Realization
{
    public class UserDialogService : IUserDialog
    {
        #region Information

        public void Error(string title, string message) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

        public void Information(string title, string message) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public void Warning(string title, string message) => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

        #endregion

        #region File operations

        public bool OpenFile(string title, out string selectedFile, string filter = "все файлы (*.*)|*.*")
        {
            var file_dialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (file_dialog.ShowDialog() != true)
            {
                selectedFile = null;
                return false;
            }

            selectedFile = file_dialog.FileName;

            return true;
        }

        public bool OpenFiles(string title, out IEnumerable<string> selectedFiles, string filter = "все файлы (*.*)|*.*")
        {
            var file_dialog = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (file_dialog.ShowDialog() != true)
            {
                selectedFiles = Enumerable.Empty<string>();
                return false;
            }

            selectedFiles = file_dialog.FileNames;

            return true;
        }

        public bool SaveFile(string title, out string selectedFile, string defaultFileName = null, string filter = "Все файлы (*.*)|*.*")
        {
            var file_dialog = new SaveFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (!string.IsNullOrWhiteSpace(defaultFileName))
                file_dialog.FileName = defaultFileName;

            if (file_dialog.ShowDialog() != true)
            {
                selectedFile = null;
                return false;
            }

            selectedFile = file_dialog.FileName;

            return true;
        }

        #endregion
    }
}
