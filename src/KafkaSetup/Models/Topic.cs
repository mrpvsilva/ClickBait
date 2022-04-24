using System.Text.Json;

namespace KafkaSetup.Models
{
    internal class Topic
    {
        public string Name { get; set; }
        public JsonDocument Key { get; set; }
        public JsonDocument Value { get; set; }
    }
}
