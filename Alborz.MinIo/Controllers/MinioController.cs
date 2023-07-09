using Cleint;
using Cleint.DataModel;
using Cleint.DataModel.ILM;
using Microsoft.AspNetCore.Mvc;
using Services.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Xml;

namespace Alborz.MinIo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinioController : ControllerBase
    {        

        private readonly ILogger<MinioController> _logger;
        private readonly MinioClient _MinioClient;

        public MinioController(ILogger<MinioController> logger, MinioClient minioClient)
        {
            _logger = logger;
            _MinioClient = minioClient;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> MakeBucket(string bucketName)
        {
            //_logger.LogError("for test");

            var location = "us-east-1";
            var endpoint = "127.0.0.1:9000";
            var accessKey = "yYQGMZ2U31fhjq4myB3Z";
            var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
            var secure = false;
            //var minio = new MinioClient()
            //    .WithEndpoint(endpoint)
            //    .WithCredentials(accessKey, secretKey)
            //    .WithSSL(secure)
            //    .Build();
            
            _MinioClient
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .WithSSL(secure)
            .Build();

            var mkBktArgs = new MakeBucketArgs()
                   .WithBucket(bucketName)
                   .WithLocation(location);

            var bktExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
            var found = await _MinioClient.BucketExistsAsync(bktExistArgs).ConfigureAwait(false);
            if (!found)
            {                   
                await _MinioClient.MakeBucketAsync(mkBktArgs).ConfigureAwait(false);
                return Ok("باکت جدید ایجاد شد");
            }
            return Ok("از قبل وجود دارد");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> PutObjectBucket(string bucketName)
        {
            try
            {
                var objectName = "my.mp3";
                var filePath = "D:\\down\\data\\my.mp3";
                //var filePath = "my.mp3";
                var contentType = "application/octet-stream";
                var endpoint = "127.0.0.1:9000";
                var accessKey = "yYQGMZ2U31fhjq4myB3Z";
                var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();

                //var putObjectArgs = new PutObjectArgs()
                //     .WithBucket(bucketName)
                //     .WithObject(objectName)
                //     .WithFileName(filePath)
                //     .WithContentType(contentType);
                //await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);



                //// Set Lifecycle configuration for the bucket
                //var lfc = new LifecycleConfiguration();
                //var lifecycleRule = new LifecycleRule();
                //lifecycleRule.Status = 1;
                //var ruleFilter = new RuleFilter();

                //ruleFilter.Prefix = "*";

                //lifecycleRule.TransitionObject.StorageClass = null;
                //lifecycleRule.TransitionObject.Days = 1;

                //lfc.Rules.Add(lifecycleRule);

                //// Create an XML element to represent the status
                //XmlDocument statusXmlElement = new XmlDocument();
                //statusXmlElement.InnerText = lifecycleRule.Status = "Status" ;



                //get status bucket name
                //GetBucketLifecycleArgs args1 = new GetBucketLifecycleArgs()
                //   .WithBucket(bucketName);
                //var findGetBucket = await minio.GetBucketLifecycleAsync(args1);
                //var status = findGetBucket.Rules[0].Status;

                //var lifecycleConfiguration = new LifecycleConfiguration()
                //{                    
                //    Rules = new Collection<LifecycleRule>()
                //    {                        
                //      new LifecycleRule()
                //      {
                //          Expiration = new Expiration()
                //          {
                //              Days = 1,
                //              ExpiredObjectDeleteMarker = true
                //          },

                //          Status = "Enabled",
                //          ID = "cikt4adovnbu8aoutkmg",
                //           Filter = new RuleFilter()
                //           {
                //               Prefix = "tmp"                             
                //           },
                //          //TransitionObject = new Transition()
                //          //{
                //          //  Days = 1,
                //          //  StorageClass = "DELETE"
                //          //},
                //      }
                //    }
                //};


                ////Create Bucket Lifecycle Configuration for the bucket
                //SetBucketLifecycleArgs args = new SetBucketLifecycleArgs()
                //                .WithBucket(bucketName)
                //                .WithLifecycleConfiguration(lifecycleConfiguration);
                //await minio.SetBucketLifecycleAsync(args);


                //// Remove Bucket Lifecycle Configuration for the bucket
                //var args = new RemoveBucketLifecycleArgs()
                //               .WithBucket(bucketName);
                //await minio.RemoveBucketLifecycleAsync(args);
                //Console.WriteLine($"Set Lifecycle for bucket {bucketName}.");                


                // List bucker              
                //var test =   await minio.ListBucketsAsync().ConfigureAwait(false); ;

                //list object                         
               var args = new ListObjectsArgs()
              .WithBucket("essi")
              .WithPrefix(null)
              .WithRecursive(true);
                var observable = minio.ListObjectsAsync(args);
                var subscription = observable.Subscribe(
                item => item.Key.ToString());



                return Ok("ok");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ArgumentOutOfRangeException(
                            "خطا", ex.Message);
            }            
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveBucket(string bucketName)
        {
            try
            {
                var objectName = "my.mp3";
                var filePath = "D:\\down\\data\\my.mp3";
                var contentType = "application/octet-stream";
                var endpoint = "127.0.0.1:9000";
                var accessKey = "yYQGMZ2U31fhjq4myB3Z";
                var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();

                var removeBucketArgs = new RemoveBucketArgs();
                removeBucketArgs.WithBucket(bucketName);
                await minio.RemoveBucketAsync(removeBucketArgs).ConfigureAwait(false);

                return Ok("عملیات با موفقیت انجام شد");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> RemoveObject(string bucketName,string objectName)
        {
            try
            {
                var endpoint = "127.0.0.1:9000";
                var accessKey = "yYQGMZ2U31fhjq4myB3Z";
                var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();
                
                var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
                removeObjectArgs.WithBucket(bucketName);
                await minio.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);

                return Ok("عملیات با موفقیت انجام شد");
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("[action]")]
        public async Task<ActionResult> CreateLink(string bucketName, string objectName)
        {
            try
            {
                var endpoint = "127.0.0.1:9000";
                var accessKey = "yYQGMZ2U31fhjq4myB3Z";
                var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();         

                PresignedGetObjectArgs presignedGetObjectArgs = new PresignedGetObjectArgs()
                                       .WithBucket(bucketName)
                                       .WithObject(objectName)
                                       .WithExpiry(60 * 60 * 24);
                string url = await minio.PresignedGetObjectAsync(presignedGetObjectArgs).ConfigureAwait(false);

                return Ok(url);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ListBucket(string bucketName)
        {
            try
            {
                var endpoint = "127.0.0.1:9000";
                var accessKey = "yYQGMZ2U31fhjq4myB3Z";
                var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();
           
                 await minio.ListBucketsAsync().ConfigureAwait(false);

                return Ok("عملیات با موفقیت انجام شد");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> LifeCyle(string bucketName)
        {
            try
            {
                var endpoint = "127.0.0.1:9000";
                var accessKey = "yYQGMZ2U31fhjq4myB3Z";
                var secretKey = "KdSbjveptox9l9zn3ZVDZSBsXeOeDdfB8xijztNv";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();

                await minio.ListBucketsAsync().ConfigureAwait(false);

                return Ok("عملیات با موفقیت انجام شد");
            }
            catch (Exception)
            {
                throw;
            }
        }

      
    }
}