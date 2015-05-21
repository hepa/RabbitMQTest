using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Messaging.Models;
using Core.Messaging.Serilaizers;
using RabbitMQ.Client;

namespace Provider
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

                    var p = new Person() {Name = "hepa"};

                    var serializer = SerializerFactory<JsonSerializer>.Instance;
                    var body = serializer.MessageToBytes(p);
                    
                    while (true)
                    {
                        Thread.Sleep(1000);
                        channel.BasicPublish("", "hello", null, body);
                        Console.WriteLine(" {1} - [x] Sent {0}", p, DateTime.UtcNow);
                    }
                                                            
                }
            }            
        }
    }
}
