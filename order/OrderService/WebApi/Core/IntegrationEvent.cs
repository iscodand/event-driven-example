namespace WebApi.Core
{
    public abstract class Event
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }

        protected Event()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }
    }
}
