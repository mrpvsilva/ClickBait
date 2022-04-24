using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using System.Text;

namespace KafkaSetup.Services
{
    internal class KsqldbService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public KsqldbService(IConfiguration config, AsyncRetryPolicy<HttpResponseMessage> retryPolicy)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(config.GetSection("KsqldbServer").Value)
            };

            _retryPolicy = retryPolicy;

            HealthCheck().Wait();
        }

        async Task HealthCheck()
        {
            await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(""));
        }

        public void ExecuteCommandsKsqldb()
        {
            using var sr = new StreamReader("./Kafka/Ksqldb/commands.ksql");
            string? ksql;
            while ((ksql = sr.ReadLine()) != null)
            {
                var data = new
                {
                    ksql,
                    streamsProperties = new { }
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync($"ksql", content).Result;

                if (response.StatusCode != System.Net.HttpStatusCode.OK ^ response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    throw new Exception($"Error on execute command {ksql}");

                Console.WriteLine($"Ksql command {ksql} executed successfully");
            }
        }
    }
}
