using ClickBait.Infra.Kafka.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Confluent.SchemaRegistry;
using Chr.Avro.Confluent;

namespace ClickBait.Infra.Kafka
{
    internal class KafkaProducer : IKafkaProducer
    {
        public Guid Id { get; }
        private readonly ProducerConfig _producerConfig;
        private readonly SchemaRegistryConfig _registryConfig;
        public KafkaProducer(IOptions<KafkaConfig> options)
        {
            Id = Guid.NewGuid();

            var config = options.Value;

            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = config.BootstrapServers,
            };

            _registryConfig = new SchemaRegistryConfig
            {
                Url = config.SchemaRegistryUrl
            };
        }
        public async Task SendMessage<TKey, TValue>(string topic, TKey key, TValue value)
        {
            using (var registry = new CachedSchemaRegistryClient(_registryConfig))
            {
                var builder = new ProducerBuilder<TKey, TValue>(_producerConfig)
                    .SetAvroValueSerializer(registry, registerAutomatically: AutomaticRegistrationBehavior.Always)
                    .SetAvroKeySerializer(registry, registerAutomatically: AutomaticRegistrationBehavior.Always)
                    .SetErrorHandler((_, error) => Console.Error.WriteLine(error.ToString()));

                using (var producer = builder.Build())
                {
                    await producer.ProduceAsync(topic, new Message<TKey, TValue>
                    {
                        Value = value,
                        Key = key
                    });
                }
            }
        }
    }
}
