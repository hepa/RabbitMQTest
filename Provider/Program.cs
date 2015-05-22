using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Messaging.Serilaizers;
using Core.Models;
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
                    channel.QueueDeclare("hello", true, false, false, null);
                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.SetPersistent(true);

                    var p = new Person() {Name = "hepa"};

                    var serializer = SerializerFactory<ProtobufSerializer>.Instance;                    
                    var i = 0;
                    
                    while (true)
                    {
                        Thread.Sleep(1000);
//                        var body = serializer.MessageToBytes("hello"+i+++"...");
                        var body = serializer.MessageToBytes(p);
                        channel.BasicPublish("", "hello", basicProperties, body);
                        Console.WriteLine(" {1} - [x] Sent {0}", "hello"+i+"...", DateTime.UtcNow);
                    }
                                                            
                }
            }            
        }
    }
}
