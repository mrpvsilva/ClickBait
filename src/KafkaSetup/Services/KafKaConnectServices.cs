using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using System.Text;

namespace KafkaSetup.Services
{
    internal class KafKaConnectServices
    {
        private readonly HttpClient _httpClient;
        private readonly IEnumerable<FileInfo> _files;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        public KafKaConnectServices(IConfiguration configuration, AsyncRetryPolicy<HttpResponseMessage> retryPolicy)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetSection("KafkaConnect").Value)
            };

            _retryPolicy = retryPolicy;

            var dir = new DirectoryInfo("./Kafka/SinkConnectors");

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Dir {dir.FullName} not found");
            }

            _files = dir.GetFiles("*.json");

            HealthCheck().Wait();
        }

        async Task HealthCheck()
        {
            await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(""));
        }

        public void DropConnectors()
        {
            foreach (var file in _files)
            {
                var connector = file.Name.Replace(".json", "");

                var response = _httpClient.DeleteAsync($"connectors/{connector}").Result;

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent ^ response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception($"Error on drop connector {connector} {response.Content.ReadAsStringAsync().Result}");

                Console.WriteLine($"Connector {connector} droped successfully");
            }
        }

        public void CreateConnectors()
        {
            foreach (var file in _files)
            {
                var connector = file.Name.Replace(".json", "");
                var data = File.ReadAllText(file.FullName);
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync($"connectors", content).Result;

                if (response.StatusCode != System.Net.HttpStatusCode.Created ^ response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception($"Error on create connector {connector} {response.Content.ReadAsStringAsync().Result}");

                Console.WriteLine($"Connector {connector} created successfully");
            }
        }

    }
}
