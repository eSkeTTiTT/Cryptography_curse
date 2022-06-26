using Cryptography_curse.Infrastructure.Commands;
using Cryptography_curse.Infrastructure.Commands.Base;
using Cryptography_curse.Services.Interfaces;
using Cryptography_curse.ViewModels.Base;
using System.IO;
using System.Windows.Input;

namespace Cryptography_curse.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Constructors

        public MainViewModel(IUserDialog dialogService, IEncryptor encryptorService)
        {
            _userDialogService = dialogService;
            _encryptorService = encryptorService;
        }

        #endregion

        #region Properties

        #region Consts

        private const string FileOpenTitle = "Выбор файла";
        private const string EncryptedFileSuffix = ".encrypted";

        #endregion

        #region Services

        private readonly IUserDialog _userDialogService;
        private readonly IEncryptor _encryptorService;

        #endregion

        private string _password = "123";
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private FileInfo _selectedFile;
        public FileInfo SelectedFile
        {
            get => _selectedFile;
            set => Set(ref _selectedFile, value);
        }

        #endregion

        #region Commands

        #region Selected File Command

        private ICommand _selectFileCommand;
        public ICommand SelectFileCommand => _selectFileCommand ??= new LambdaCommand(OnSelectedFileCommandExecute);

        private void OnSelectedFileCommandExecute(object obj)
        {
            if (_userDialogService.OpenFile(FileOpenTitle, out string selectedFileName))
            {
                var selectedFile = new FileInfo(selectedFileName);
                SelectedFile = selectedFile.Exists ? selectedFile : null;
            }
        }

        #endregion

        #region Encrypt Commad

        private ICommand _encryptComamnd;
        public ICommand EncryptCommand => _encryptComamnd ??= new LambdaCommand(OnEncryptCommandExecute, CanEncryptCommandExecute);

        private bool CanEncryptCommandExecute(object obj) =>
            (obj is FileInfo file
            && file.Exists
            || SelectedFile != null)
            && !string.IsNullOrWhiteSpace(Password);

        private async void OnEncryptCommandExecute(object obj)
        {
            var file = obj as FileInfo ?? SelectedFile;

            if (file is null)
            {
                return;
            }

            var defaultFileName = file.FullName + EncryptedFileSuffix;
            if (!_userDialogService.SaveFile("Выбор файла для сохранения", out var destinationPath, defaultFileName))
            {
                return;
            }

            (EncryptCommand as Command).Executable = false;
            (DecryptCommand as Command).Executable = false;
            await _encryptorService.EncryptAsync(file.FullName, destinationPath, Password);
            (EncryptCommand as Command).Executable = true;
            (DecryptCommand as Command).Executable = true;

            _userDialogService.Information("Шифрование", "Шифрование файла успешно завершено!");
        }

        #endregion

        #region Decrypt Command

        private ICommand _decryptComamnd;
        public ICommand DecryptCommand => _decryptComamnd ??= new LambdaCommand(OnDecryptCommandExecute, CanDecryptCommandExecute);

        private bool CanDecryptCommandExecute(object obj) =>
            (obj is FileInfo file
            && file.Exists
            || SelectedFile != null)
            && !string.IsNullOrWhiteSpace(Password);

        private async void OnDecryptCommandExecute(object obj)
        {
            var file = obj as FileInfo ?? SelectedFile;

            if (file is null)
            {
                return;
            }

            var defaultFileName = file.FullName.EndsWith(EncryptedFileSuffix)
                ? file.FullName.Substring(0, file.FullName.Length - EncryptedFileSuffix.Length)
                : file.FullName;
            if (!_userDialogService.SaveFile("Выбор файла для сохранения", out var destinationPath, defaultFileName))
            {
                return;
            }

            (EncryptCommand as Command).Executable = false;
            (DecryptCommand as Command).Executable = false;
            bool result = await _encryptorService.DecryptAsync(file.FullName, destinationPath, Password);
            (EncryptCommand as Command).Executable = true;
            (DecryptCommand as Command).Executable = true;

            if (result)
            {
                _userDialogService.Information("Шифрование", "Дешифровка файла выполнено успешно!");
            }
            else
            {
                _userDialogService.Warning("Шифрование", "Ошибка при дешифровке файла: указан неверный пароль.");
            }
        }

        #endregion

        #endregion
    }
}
