using Cleint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;


namespace Services.Services
{
    public class RabbitMqServices : IRabbitMqServices
    {

        public async Task Test()
        {
            throw new NotImplementedException("khata");
        }
        public async Task GetRabbitMessage()
        {
            string QueueName = "inbox";
            int i = 0;
            long messageIndex = 0;
            // long serverIndex = 0;

            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "127.0.0.1" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    var stopWatch = new System.Diagnostics.Stopwatch();
                    stopWatch.Start();

                    channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object> { { "x-max-priority", 50 } });



                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(model: channel);

                    // using RabbitMQ.Client;
                    channel.BasicConsume
                        (queue: QueueName,
                        autoAck: false,
                        arguments: null,
                        consumer: consumer);

                    consumer.Received += (model, eventArgs) =>
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
                            //var body = eventArgs.Body.ToArray();
                            //throw new Exception(response);
                            message = System.Text.Encoding.UTF8.GetString(body);

                            //if (message == "essi")
                            //{
                            //    var body1 = Encoding.UTF8.GetBytes(message);
                            //    var ReplyQueueName = channel.QueueDeclare(queue: string.Empty).QueueName;
                            //    properties.ReplyTo = ReplyQueueName;
                            //    channel.BasicPublish(exchange: "", routingKey: "inbox", basicProperties: properties, body1);
                            //    System.Console.WriteLine($"{message},not compelete");
                            //}

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
                            channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: true);

                            //channel.BasicReject(eventArgs.DeliveryTag, false);//بازگشت به صف---اگر فالس باشد از صف دور ریخته میشود

                            //channel.BasicNack(eventArgs.DeliveryTag, false, false);

                            channel.QueueDeclare(queue: "inbox", durable: true, exclusive: false, autoDelete: false, arguments: null);

                            channel.BasicPublish(exchange: "e5", routingKey: "", basicProperties: properties, body: body);



                            System.Console.WriteLine($"{message},compelete,{i = i + 1}");
                            if (i == 10000)
                            {
                                stopWatch.Stop();
                                TimeSpan ts = stopWatch.Elapsed;
                                System.Console.Write(ts);
                            }
                        }
                    };

                    // این دو دستور ذیل باید دقیقا اینجا نوشته شوند
                    System.Console.Write("Press [ENTER] to exit...");
                    System.Console.ReadLine();
                }
            }
            System.Console.WriteLine("Consumer Stoped!");
        }
    }
}
