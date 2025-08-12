namespace WebApi.Core.Orders.Events
{
    public class OrderCreatedEvent : Event
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string User { get; set; }
    }
}
