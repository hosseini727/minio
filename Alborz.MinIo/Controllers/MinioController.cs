using Cleint;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Alborz.MinIo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinioController : ControllerBase
    {        

        private readonly ILogger<MinioController> _logger;    
        
        public MinioController(ILogger<MinioController> logger)
        {
            _logger = logger;            
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> MakeBucket(string bucketName)
        {
            _logger.LogError("for test");

            var location = "us-east-1";
            var endpoint = "127.0.0.1:9000";
            var accessKey = "wO8pTUIpzqfcYbqZTLqm";
            var secretKey = "CDKiLYVaSIGCcowbXGHQXE2FP7aatRm9eDVfTvND";
            var secure = false;
            var minio = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(secure)
                .Build();

            var mkBktArgs = new MakeBucketArgs()
                   .WithBucket(bucketName)
                   .WithLocation(location);

            var bktExistArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
            var found = await minio.BucketExistsAsync(bktExistArgs).ConfigureAwait(false);
            if (!found)
            {                   
                await minio.MakeBucketAsync(mkBktArgs).ConfigureAwait(false);
                return Ok("باکت جدید ایجاد شد");

            }
            return Ok("از قبل وجود دارد");
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> PutObjectBucket()
        {
            try
            {
                var bucketName = "test333";
                var objectName = "my.mp3";
                var filePath = "D:\\down\\data\\my.mp3";
                var contentType = "application/octet-stream";
                var endpoint = "127.0.0.1:9000";
                var accessKey = "wO8pTUIpzqfcYbqZTLqm";
                var secretKey = "CDKiLYVaSIGCcowbXGHQXE2FP7aatRm9eDVfTvND";
                var secure = false;
                var minio = new MinioClient()
                    .WithEndpoint(endpoint)
                    .WithCredentials(accessKey, secretKey)
                    .WithSSL(secure)
                    .Build();

                var putObjectArgs = new PutObjectArgs()
                     .WithBucket(bucketName)
                     .WithObject(objectName)
                     .WithFileName(filePath)
                     .WithContentType(contentType);
                await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

                return Ok("عملیات با موفقیت انجام شد");
            }
            catch (Exception)
            {
                throw;
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
                var accessKey = "wO8pTUIpzqfcYbqZTLqm";
                var secretKey = "CDKiLYVaSIGCcowbXGHQXE2FP7aatRm9eDVfTvND";
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
                var accessKey = "wO8pTUIpzqfcYbqZTLqm";
                var secretKey = "CDKiLYVaSIGCcowbXGHQXE2FP7aatRm9eDVfTvND";
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
    }
}