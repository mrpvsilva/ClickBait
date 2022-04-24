using System.Text;

namespace KafkaSetup.Services
{
    internal class KafKaConnectService
    {
        private readonly HttpClient _httpClient;
        private readonly IEnumerable<FileInfo> _files;

        public KafKaConnectService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            var dir = new DirectoryInfo("./Kafka/SinkConnectors");

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Dir {dir.FullName} not found");
            }

            _files = dir.GetFiles("*.json");

            HealthCheck();
        }

        HttpResponseMessage HealthCheck() => _httpClient.GetAsync("").Result;

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
