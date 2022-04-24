using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Confluent.SchemaRegistry;
using KafkaSetup.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace KafkaSetup.Services
{
    internal class KafkaService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public KafkaService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
            HealthCheck();
        }

        HttpResponseMessage HealthCheck() => _httpClient.GetAsync("").Result;

        public void CreateTopics()
        {
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = _config.GetSection("BootstrapServers").Value
            };

            var dir = new DirectoryInfo("./Kafka/Topics");

            foreach (var file in dir.GetFiles("*.json"))
            {
                using var adminClient = new AdminClientBuilder(adminConfig).Build();

                string json = File.ReadAllText(file.FullName);

                var topic = JsonSerializer.Deserialize<Topic>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                try
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(20));

                    if (!metadata.Topics.Any(x => x.Topic == topic.Name))
                    {
                        adminClient
                        .CreateTopicsAsync(new TopicSpecification[]
                        {
                            new TopicSpecification
                            {
                                Name = topic.Name,
                                ReplicationFactor = 1,
                                NumPartitions = 1
                            }
                        })
                       .Wait();

                        Console.WriteLine($"Topic {topic.Name} created successfully");
                    }

                    CreateSchema($"{topic.Name}-key", topic.Key);
                    CreateSchema($"{topic.Name}-value", topic.Value);
                }
                catch (CreateTopicsException e)
                {
                    Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
                }
            }
        }

        void CreateSchema(string subject, JsonDocument jsonSchema)
        {
            try
            {
                var schemaRegistryConfig = new SchemaRegistryConfig
                {
                    Url = _config.GetSection("SchemaRegistry").Value
                };

                using var client = new CachedSchemaRegistryClient(schemaRegistryConfig);
                var schema = new Schema(JsonSerializer.Serialize(jsonSchema), SchemaType.Avro);

                if (!client.IsCompatibleAsync(subject, schema).Result)
                {
                    var response = _httpClient.DeleteAsync($"subjects/{subject}").Result;

                    if (response.StatusCode != System.Net.HttpStatusCode.OK ^ response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        Console.Error.WriteLine(response.Content.ReadAsStringAsync().Result);
                        throw new Exception($"Error on delete subject {subject}");
                    }
                }

                _ = client.RegisterSchemaAsync(subject, schema).Result;

                Console.WriteLine($"Subject created {subject} successfully");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on create subject {subject}", ex);
            }
        }
    }
}
