using Microsoft.Extensions.DependencyInjection;
using System;

namespace Cryptography_curse.ViewModels.Locator
{
    public class Locator
    {
        public MainViewModel MainViewModel => App.Host.Services.GetRequiredService<MainViewModel>();
    }
}
