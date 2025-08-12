using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApi.Core.Orders.Comands;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/orders/")]
    public class OrderController : ControllerBase
    {
        private readonly IProducer<string, string> _producer;

        public OrderController()
        {
            ProducerConfig config = new()
            {
                BootstrapServers = "localhost:19090",
                Acks = Acks.All,
                EnableIdempotence = true,
                CompressionType = CompressionType.Snappy,
                LingerMs = 5
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderCommand command)
        {
            string topic = "CreateOrderEventTopic";

            string message = JsonSerializer.Serialize(command);

            try
            {
                Message<string, string> kafkaMessage = new()
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                };

                var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);

                Console.WriteLine($"Delivered event to Kafka: {deliveryResult.TopicPartitionOffset}");

                return Ok("Order created.");
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Failed to deliver event: {ex.Message}");
                throw;
            }
        }
    }
}