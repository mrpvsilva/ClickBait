namespace ClickBait.Domain.Entities
{
    public class Click
    {
        public DateTime DateTime { get; }
        public Guid PostId { get; set; }

        public Click()
        {
            DateTime = DateTime.UtcNow;
        }
    }
}
