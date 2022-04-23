using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using ClickBait.Application.Services;
using ClickBait.Infra.Services;
using ClickBait.Infra.Kafka;
using ClickBait.Infra.Kafka.Options;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using ClickBait.Domain.Repositories;
using ClickBait.Infra.Repositories;
using System.Reflection;

namespace ClickBait.Infra.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR();
            services.AddKafkaProducer(config);
            services.AddServices();
            services.AddRepositories();
            services.AddClickBaitContext(config);
            services.AddAutoMapper();

            return services;
        }

        private static IServiceCollection AddClickBaitContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<Data.ClickBaitContext>(x => x.UseMySql(connectionString, serverVersion));
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
            services.Scan(x => x.FromAssemblyOf<PostService>()
                                .AddClasses(classes => classes.AssignableTo<IService>())
                                .AsImplementedInterfaces()
                                .WithScopedLifetime());

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.Scan(x => x.FromAssemblyOf<PostRepository>()
                                .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                                .AsImplementedInterfaces()
                                .WithScopedLifetime());

            return services;
        }

        private static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mapper.AutoMapperProfile));
            return services;
        }
    }
}
