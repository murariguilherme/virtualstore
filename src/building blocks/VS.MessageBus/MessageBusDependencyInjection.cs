using Microsoft.Extensions.DependencyInjection;
using System;

namespace VS.MessageBus
{
    public static class MessageBusDependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException();

            services.AddSingleton<IMessageBus>(new MessageBus(connectionString));

            return services;
        }
    }
}
