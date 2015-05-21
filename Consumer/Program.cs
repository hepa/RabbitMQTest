using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Messaging.Models;
using Core.Messaging.Serilaizers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");
                    var serializer = SerializerFactory<JsonSerializer>.Instance;

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();

                        var message = serializer.BytesToMessage<Person>(ea.Body);
                        Console.WriteLine(" {1} - [x] Received {0}", message, DateTime.UtcNow);
                    }
                }
            }
        }
    }
}
