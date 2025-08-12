namespace WebApi.Core.Orders.Comands
{
    public class CreateOrderCommand
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string User { get; set; }
    }
}
