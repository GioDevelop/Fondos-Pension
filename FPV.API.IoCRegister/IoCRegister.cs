using FPV.API.Business.Interfaces;
using FPV.API.Business.Services;
using FPV.API.Core.Context;
using FPV.API.Core.Repositories.Generic.Interfaces;
using FPV.API.Core.Repositories.Generic;
using FPV.API.Core.Repositories.GenericWorker;
using FPV.API.Core.Repositories.GenericWorker.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;
using FPV.Common.Helper.Interfaces;
using FPV.API.Business.interfaces;
using FPV.Common.IntegrationApi.MailingApi.Interfaces;
using FPV.Common.IntegrationApi.MailingApi;

namespace FPV.API.IoCRegister
{
    public class IoCRegister
    {
        public static IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IUtility, Common.Helper.Util>();
            services.AddScoped<IFundServices, FundServices>();
            services.AddScoped<INotificationServices, NotificationService>();
            services.AddScoped<IMailingApi, MailingApiService>();
            return services;
        }
        public static IServiceCollection AddRepository(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IGenericWorker, GenericWorker>();

            return services;
        }

        public static IServiceCollection AddDbContext(IServiceCollection services, string defaultConnection)
        {
            services.AddDbContext<AmarisDbContext>(p => p.UseSqlServer(defaultConnection, b => b.MigrationsAssembly("Big.Emtelco.EmtelcoPoints")), ServiceLifetime.Scoped);

            return services;
        }
    }
}
