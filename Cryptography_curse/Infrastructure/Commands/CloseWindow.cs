using Cryptography_curse.Infrastructure.Commands.Base;
using System.Windows;

namespace Cryptography_curse.Infrastructure.Commands
{
    public sealed class CloseWindow : Command
    {
        protected override void Execute(object parameter) => 
            (parameter as Window ?? App.FocusWindow ?? App.ActiveWindow)?.Close();
    }
}
