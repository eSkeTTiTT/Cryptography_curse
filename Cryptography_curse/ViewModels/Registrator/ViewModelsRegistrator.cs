using Microsoft.Extensions.DependencyInjection;

namespace Cryptography_curse.ViewModels.Registrator
{
    public static class ViewModelsRegistrator
    {
        public static IServiceCollection RegisterViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MainViewModel>();

            return services;
        }
    }
}
