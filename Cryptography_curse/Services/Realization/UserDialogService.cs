using Cryptography_curse.Services.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cryptography_curse.Services.Realization
{
    public class UserDialogService : IUserDialog
    {
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
    }
}
