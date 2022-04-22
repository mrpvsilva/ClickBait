namespace ClickBait.Infra.Kafka
{
    interface IKafkaProducer
    {
        Task SendMessage<TKey, TValue>(string topic, TKey key, TValue value);
    }
}
