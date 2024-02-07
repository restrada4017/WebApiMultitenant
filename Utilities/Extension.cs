
using GPT.Application.Contracts.Infrastructure.Utilities;
using GPT.Utilities.Cache;
using GPT.Utilities.Email;
using GPT.Utilities.Encrypt;
using GPT.Utilities.Keys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GPT.Utilities
{
    public static class Extension
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services)
        {
            services.AddTransient<IMail, SmtpMail>();
            services.AddTransient<IEncrypt, EncryptData>();
            services.AddTransient<ICache, CacheRedis>();
            services.AddTransient<IKeys, AzureKeyvault>();
           
            return services;
        }
    }
}
