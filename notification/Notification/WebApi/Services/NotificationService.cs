using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Services
{
    public class NotificationService : INotificationService, IHostedService
    {
        private IConsumer<string, string> _consumer;
        private Task _consumerTask;
        private CancellationTokenSource _cts;

        public NotificationService()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:19090",
                GroupId = "notification-service-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("CreateOrderEventTopic");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _consumerTask = Task.Run(() => ConsumeLoop(_cts.Token), cancellationToken);
            return Task.CompletedTask;
        }

        private void ConsumeLoop(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    Console.WriteLine($"Customer notified! {cr.Message.Key}:{cr.Message.Value}");
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _consumer.Close();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return _consumerTask ?? Task.CompletedTask;
        }

        public void SendOrderCreated()
        {
            throw new NotImplementedException();
        }
    }
}
