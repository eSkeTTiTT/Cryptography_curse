using Cryptography_curse.Services.Interfaces;
using Cryptography_curse.Services.Realization;
using Microsoft.Extensions.DependencyInjection;

namespace Cryptography_curse.Services.Registrator
{
    public static class ServicesRegistrator
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUserDialog, UserDialogService>();
            services.AddTransient<IEncryptor, Rfc2898Encryptor>();

            return services;
        }
    }
}
