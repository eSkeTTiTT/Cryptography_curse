using System;
using System.Windows.Input;

namespace Cryptography_curse.Infrastructure.Commands.Base
{
    public abstract class Command : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        private bool _executable;
        public bool Executable
        {
            get => _executable;
            set
            {
                if (_executable == value)
                {
                    return;
                }

                _executable = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        bool ICommand.CanExecute(object parameter) => Executable && CanExecute(parameter);

        void ICommand.Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                Execute(parameter);
            }
        }

        protected virtual bool CanExecute(object parameter) => true;

        protected abstract void Execute(object parameter);
    }
}
