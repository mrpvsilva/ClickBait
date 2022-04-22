using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Confluent.Kafka;
using ClickBait.Application.Services;
using ClickBait.Infra.Services;
using ClickBait.Infra.Kafka;
using ClickBait.Infra.Kafka.Options;

namespace ClickBait.Infra.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR();
            services.AddKafkaProducer(config);
            services.AddServices();

            return services;
        }

        private static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Application.Handlers.RegisterClickCommandHandler));
            return services;
        }

        private static IServiceCollection AddKafkaProducer(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<KafkaConfig>(x => config.Bind(nameof(KafkaConfig), x));
            services.AddSingleton<IKafkaProducer, KafkaProducer>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClickService, ClickService>();
            return services;
        }
    }
}
