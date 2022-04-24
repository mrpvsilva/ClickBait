// See https://aka.ms/new-console-template for more information

using KafkaSetup.Services;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

var env = Environment.GetEnvironmentVariable("DOTNET_ENV");

var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables()
                .Build();

var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => !r.IsSuccessStatusCode)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(20, retryCount => TimeSpan.FromMilliseconds(retryCount * 1000), (result, timeSpan, retryCount, context) =>
                {
                    Console.Error.WriteLine(result?.Exception?.Message);
                    Console.WriteLine($"Service delivery attempt {retryCount} failed, next attempt in {timeSpan.TotalMilliseconds} ms.");
                });

var kafkaConnectService = new KafKaConnectServices(config, retryPolicy);
var kafkaService = new KafkaService(config, retryPolicy);
var ksqldbService = new KsqldbService(config, retryPolicy);

kafkaService.CreateTopics();
kafkaConnectService.DropConnectors();
ksqldbService.ExecuteCommandsKsqldb();
kafkaConnectService.CreateConnectors();

Console.WriteLine("Finish");





