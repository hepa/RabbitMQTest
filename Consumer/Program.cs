using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Messaging.Serilaizers;
using Core.Models;
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
                    channel.QueueDeclare("hello", true, false, false, null);
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", false, consumer);

                    Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");
                    var serializer = SerializerFactory<ProtobufSerializer>.Instance;

                    while (true)
                    {
                        var ea = consumer.Queue.Dequeue();

                        var message = serializer.BytesToMessage<Person>(ea.Body);
                        Console.WriteLine(" {1} - [x] Received {0}", message, DateTime.UtcNow);

//                        int dots = message.Split('.').Length - 1;
//                        Thread.Sleep(dots * 1000);

                        Console.WriteLine(" [x] Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
