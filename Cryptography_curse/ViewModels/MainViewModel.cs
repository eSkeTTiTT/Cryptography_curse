using Cryptography_curse.Infrastructure.Commands;
using Cryptography_curse.Services.Interfaces;
using Cryptography_curse.ViewModels.Base;
using System;
using System.IO;
using System.Windows.Input;

namespace Cryptography_curse.ViewModels
{
    public class MainViewModel : ViewModel
    {
        #region Constructors

        public MainViewModel(IUserDialog dialogService)
        {
            _userDialogService = dialogService;
        }

        #endregion

        #region Properties

        #region Consts

        private const string FileOpenTitle = "Выбор файла";

        #endregion

        #region Services

        private readonly IUserDialog _userDialogService;

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

        #endregion
    }
}
