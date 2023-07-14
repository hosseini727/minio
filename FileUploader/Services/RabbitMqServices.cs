using Cleint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System.Collections.ObjectModel;
using Cleint.DataModel.Tags;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

namespace Services.Services
{
    public class RabbitMqServices : IDisposable //: IRabbitMqServices
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public const string queueName = "ef";

        public RabbitMqServices(string hostName, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        //private readonly MinioClient _MinioClient;


        //public RabbitMqServices(MinioClient minioClient)
        //{
        //    _MinioClient = minioClient;
        //}

        public void ConsumeMessages()
        {
            var consumer = new EventingBasicConsumer(_channel);
            //consumer.Received += (model, ea) =>
            //{
            //    var body = ea.Body.ToArray();
            //    var message = Encoding.UTF8.GetString(body);
            //    Console.WriteLine("Received message: {0}", message);
            //};

            consumer.Received += async (model, eventArgs) =>
                        {
                            var properties = eventArgs.BasicProperties;
                            var replyProperties =
                                _channel.CreateBasicProperties();

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
                            }
                            finally
                            {
                                var responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
                                _channel.BasicPublish
                                    (exchange: string.Empty,
                                     routingKey: _channel.QueueDeclare(queue: string.Empty).QueueName,
                                    //routingKey: properties.ReplyTo,
                                    basicProperties: replyProperties,
                                    body: responseBytes);
                                _channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

                                _channel.QueueDeclare(queue: "ef", durable: true, exclusive: false, autoDelete: false, arguments: null);

                                var removeTagObject = RemoveTagObject(response);
                            }
            };

            _channel.BasicConsume(queue: queueName,
                                  autoAck: false,
                                  arguments: null,
                                  consumer: consumer);
        }

        //public void Consume()
        //{
        //    int i = 0;
        //    long messageIndex = 0;
        //    var factory = new RabbitMQ.Client.ConnectionFactory() { HostName = "127.0.0.1" };

        //    using (var connection = factory.CreateConnection())
        //    {
        //        using (var channel = connection.CreateModel())
        //        {

        //            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        //            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        //            var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(model: channel);

        //            // using RabbitMQ.Client;
        //            channel.BasicConsume
        //                (queue: queueName,
        //                autoAck: false,
        //                arguments: null,
        //                consumer: consumer);

        //            consumer.Received += async (model, eventArgs) =>
        //            {
        //                var properties = eventArgs.BasicProperties;
        //                var replyProperties =
        //                    channel.CreateBasicProperties();

        //                replyProperties.CorrelationId = properties.CorrelationId;
        //                string response = "";
        //                string message = "";
        //                var body = eventArgs.Body.ToArray();
        //                try
        //                {
        //                    message = System.Text.Encoding.UTF8.GetString(body);
        //                    response = message;
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    response = 0.ToString();
        //                }
        //                finally
        //                {
        //                    var responseBytes = System.Text.Encoding.UTF8.GetBytes(response);
        //                    channel.BasicPublish
        //                        (exchange: string.Empty,
        //                         routingKey: channel.QueueDeclare(queue: string.Empty).QueueName,
        //                        //routingKey: properties.ReplyTo,
        //                        basicProperties: replyProperties,
        //                        body: responseBytes);
        //                    channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);

        //                    channel.QueueDeclare(queue: "ef", durable: true, exclusive: false, autoDelete: false, arguments: null);

        //                    var removeTagObject = RemoveTagObject(response);

        //                }
        //            };


        //            System.Console.ReadKey();
        //        }
        //    }
        //    System.Console.WriteLine("Consumer Stoped!");

       
        //}


        public bool RemoveTagObject(string bucketName)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            var collection = new Collection<Cleint.DataModel.Tags.Tag>();
            var tag = new Tag();
            tag.Key = "1";
            tag.Value = "tmp";
            collection.Add(tag);
            var tagging = new Tagging();
            var tagset = new TagSet();
            tagset.Tag = collection;
            tagging.TaggingSet = tagset;
            var objectName = "111.png";
            //var filePath = "D:\\down\\data\\my.png";
            var filePath = "C:/111.png";
            var contentType = "application/octet-stream";
            var endpoint = "127.0.0.1:9000";
            var accessKey = "lnYeEijms41YL48gmXXt";
            var secretKey = "XCXQAXM9YNmCGp03Pu879wu1aJ4rI2qIV4WGGkHX";
            var secure = false;
            var minio = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(secure)
                .Build();

            try
            {
                var args = new RemoveObjectTagsArgs()
                               .WithBucket("test")
                               .WithObject("111.png");
                var ff = minio.RemoveObjectTagsAsync(args);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _channel.Dispose();
            _channel.Dispose();
        }
    }
}
