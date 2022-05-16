using Cryptography_curse.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptography_curse.Infrastructure.Commands
{
    public class LambdaCommand : Command
    {
        #region Properties

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion

        #region Constructors

        public LambdaCommand(Action execute, Func<bool> canExecute)
            : this(p => execute(), canExecute is null ? (Predicate<object>)null : p => canExecute())
        {
        }

        public LambdaCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion

        protected override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        protected override void Execute(object parameter) => _execute(parameter);
    }
}
