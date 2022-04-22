using ClickBait.Application.Services;
using ClickBait.Domain.Entities;
using ClickBait.Infra.Kafka;
using ClickBait.Infra.Properties;

namespace ClickBait.Infra.Services
{
    internal class ClickService : IClickService
    {
        private readonly IKafkaProducer _kafkaProducer;
        public ClickService(IKafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        public async Task<Click> Register(Click click)
        {
            await _kafkaProducer.SendMessage(Resources.ClickBaitTopic, click.PostId, click);
            return click;
        }
    }
}
