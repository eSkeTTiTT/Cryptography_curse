using System.Collections.Generic;

namespace Cryptography_curse.Services.Interfaces
{
    public interface IUserDialog
    {
        bool OpenFile(string title, out string selectedFile, string filter = "все файлы (*.*)|*.*");
        bool OpenFiles(string title, out IEnumerable<string> selectedFiles, string filter = "все файлы (*.*)|*.*");
        bool SaveFile(string title, out string selectedFile, string defaultFileName = null, string filter = "Все файлы (*.*)|*.*");
    }
}
