// See https://aka.ms/new-console-template for more information

using KafkaSetup.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;


var serviceProvider = ConfigureServices();

var kafkaService = serviceProvider.GetRequiredService<KafkaService>();
var kafkaConnectService = serviceProvider.GetRequiredService<KafKaConnectService>();
var ksqldbService = serviceProvider.GetRequiredService<KsqldbService>();

kafkaService.CreateTopics();
kafkaConnectService.DropConnectors();
ksqldbService.ExecuteCommandsKsqldb();
kafkaConnectService.CreateConnectors();

Console.WriteLine("Finish");


static IServiceProvider ConfigureServices()
{
    var env = Environment.GetEnvironmentVariable("DOTNET_ENV");

    var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{env}.json", true)
                    .AddEnvironmentVariables()
                    .Build();

    var services = new ServiceCollection();

    services.AddSingleton<IConfiguration>(x => config);

    services.AddHttpClient<KafKaConnectService>(x => x.BaseAddress = new Uri(config.GetSection("KafkaConnect").Value))
        .AddPolicyHandler(GetRetryPolicy());

    services.AddHttpClient<KafkaService>(x => x.BaseAddress = new Uri(config.GetSection("SchemaRegistry").Value))
        .AddPolicyHandler(GetRetryPolicy());

    services.AddHttpClient<KsqldbService>(x => x.BaseAddress = new Uri(config.GetSection("KsqldbServer").Value))
        .AddPolicyHandler(GetRetryPolicy());

    return services.BuildServiceProvider();
}

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r => !r.IsSuccessStatusCode)
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(15, retryAttempt => TimeSpan.FromSeconds(1 * retryAttempt), (result, timeSpan, retryCount, context) =>
            {
                Console.Error.WriteLine(result?.Exception?.Message);
                Console.WriteLine($"Attempt {retryCount} failed, next attempt in {timeSpan.TotalMilliseconds} ms.");
            });
}





