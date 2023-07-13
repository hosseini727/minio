using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;

namespace Moon
{
    public static class Program
    {
        public const string QueueName = "q1";
        //section two
        private static void Main()
        {
            int i = 0;
            long messageIndex = 0;
            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "127.0.0.1" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();

                    channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(model: channel);

                    // using RabbitMQ.Client;
                    channel.BasicConsume
                        (queue: QueueName,
                        autoAck: false,
                        arguments: null,
                        consumer: consumer);

                    consumer.Received += async (model, eventArgs) =>
                    {
                        var properties = eventArgs.BasicProperties;
                        var replyProperties =
                            channel.CreateBasicProperties();

                        replyProperties.CorrelationId = properties.CorrelationId;
                        string response = "";
                        string message = "";
                        var body = eventArgs.Body.ToArray();
                        try
                        {                        
                            message = System.Text.Encoding.UTF8.GetString(body);                                                      
                            response = message;
                        }
                        catch (System.Exception ex)
                        {
                            response = 0.ToString();
                            System.Console.WriteLine($"Error Message: { ex.Message }");
                        }
                        finally
                        {

                            var responseBytes =
                                System.Text.Encoding.UTF8.GetBytes(response);

                            channel.BasicPublish
                                (exchange: string.Empty,
                                 routingKey: channel.QueueDeclare(queue: string.Empty).QueueName,
                                //routingKey: properties.ReplyTo,
                                basicProperties: replyProperties,
                                body: responseBytes);
                            channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                            channel.QueueDeclare(queue: "q1", durable: true, exclusive: false, autoDelete: false, arguments: null);
                      
                            System.Console.WriteLine($"{message},compelete,{i = i + 1}");                            
                        }
                    };
                    System.Console.Write("Press [ENTER] to exit...");
                    System.Console.ReadLine();
                }
            }
            System.Console.WriteLine("Consumer Stoped!");
        }        
    }
}

