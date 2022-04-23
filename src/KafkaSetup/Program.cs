// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using System.Text;

var env = Environment.GetEnvironmentVariable("DOTNET_ENV");

var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{env}.json", true)
                .AddEnvironmentVariables()
                .Build();

var clientKafkaConnect = new HttpClient
{
    BaseAddress = new Uri(config.GetSection("KafkaConnect").Value)
};

CreateTopics(config);
DropConnectors(clientKafkaConnect);
ExecuteCommandsKsqldb(config);
UpsertSchemas(config);
CreateConnectors(clientKafkaConnect);

Console.WriteLine("Finish");



static void CreateTopics(IConfiguration config)
{
    var adminConfig = new AdminClientConfig
    {
        BootstrapServers = config.GetSection("BootstrapServers").Value
    };

    using var sr = new StreamReader("./Kafka/Topics.txt");

    using (var adminClient = new AdminClientBuilder(adminConfig).Build())
    {
        string? topic;

        while ((topic = sr.ReadLine()) != null)
        {
            try
            {
                var metadata = adminClient.GetMetadata(topic, TimeSpan.FromSeconds(10));

                if (metadata.Topics.Any(x => x.Error.Code == ErrorCode.LeaderNotAvailable))
                {
                    adminClient
                    .CreateTopicsAsync(new TopicSpecification[]
                    {
                    new TopicSpecification { Name = topic, ReplicationFactor = 1, NumPartitions = 1 }
                    },
                    new CreateTopicsOptions
                    {

                    })
                   .Wait();
                    Console.WriteLine($"Topic {topic} created successfully");
                }
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
        }
    }
}
static void DropConnectors(HttpClient clientKafkaConnect)
{
    var dir = new DirectoryInfo("./Kafka/SinkConnectors");

    foreach (var file in dir.GetFiles("*.json"))
    {
        var connector = file.Name.Replace(".json", "");

        var response = clientKafkaConnect.DeleteAsync($"connectors/{connector}").Result;

        if (response.StatusCode != System.Net.HttpStatusCode.NoContent ^ response.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new Exception($"Error on drop connector {connector} {response.Content.ReadAsStringAsync().Result}");


        Console.WriteLine($"Connector {connector} droped successfully");
    }
}

static void ExecuteCommandsKsqldb(IConfiguration config)
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(config.GetSection("KsqldbServer").Value)
    };

    using (var sr = new StreamReader("./Kafka/Ksqldb/commands.ksql"))
    {
        string? ksql;
        while ((ksql = sr.ReadLine()) != null)
        {
            var data = new
            {
                ksql,
                streamsProperties = new { }
            };

            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync($"ksql", content).Result;

            if (response.StatusCode != System.Net.HttpStatusCode.OK ^ response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception($"Error on execute command {ksql}");


            Console.WriteLine($"Ksql command {ksql} executed successfully");
        }
    }
}

static void UpsertSchemas(IConfiguration config)
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(config.GetSection("SchemaRegistry").Value)
    };

    var dir = new DirectoryInfo("./Kafka/Schema-Registry");

    foreach (var file in dir.GetFiles("*.json", SearchOption.AllDirectories))
    {
        var subject = file.Name.Replace(".json", "");
        var schema = File.ReadAllText(file.FullName);

        var data = new
        {
            schema,
            schemaType = "AVRO"
        };

        var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

        var response = httpClient.PostAsync($"subjects/{subject}/versions", content).Result;

        if (response.StatusCode != System.Net.HttpStatusCode.OK ^ response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            throw new Exception($"Error on create subject {subject}");


        Console.WriteLine($"Subject created {subject} successfully");
    }
}

static void CreateConnectors(HttpClient clientKafkaConnect)
{
    var dir = new DirectoryInfo("./Kafka/SinkConnectors");

    foreach (var file in dir.GetFiles("*.json"))
    {
        var connector = file.Name.Replace(".json", "");
        var data = File.ReadAllText(file.FullName);
        var content = new StringContent(data, Encoding.UTF8, "application/json");

        var response = clientKafkaConnect.PostAsync($"connectors", content).Result;

        if (response.StatusCode != System.Net.HttpStatusCode.Created ^ response.StatusCode == System.Net.HttpStatusCode.NotFound)
            throw new Exception($"Error on create connector {connector} {response.Content.ReadAsStringAsync().Result}");


        Console.WriteLine($"Connector {connector} created successfully");
    }
}