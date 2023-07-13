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
        public  string GetRabbitMessage()
        {
            string QueueName = "q1";

            var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "127.0.0.1" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {               
                    channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                    var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(model: channel);                    
                    channel.BasicConsume
                        (queue: QueueName,
                        autoAck: false,
                        arguments: null,
                        consumer: consumer);
                    BasicGetResult result = channel.BasicGet(queue: "q1", autoAck: false);
                    var message = System.Text.Encoding.UTF8.GetString(result.Body.ToArray());
                    var responseBytes = System.Text.Encoding.UTF8.GetBytes(message);
                    var replyProperties =channel.CreateBasicProperties();
                    channel.BasicPublish
                        (exchange: string.Empty,
                         routingKey: channel.QueueDeclare(queue: string.Empty).QueueName,
                        //routingKey: properties.ReplyTo,
                        basicProperties: replyProperties,
                        body: responseBytes);
                    channel.BasicAck(deliveryTag: result.DeliveryTag, multiple: false);
                    channel.QueueDeclare(queue: "q1", durable: true, exclusive: false, autoDelete: false, arguments: null);
                    return message;                    
                }
            }

        }            
        
    }
}
