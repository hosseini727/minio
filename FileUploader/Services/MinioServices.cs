using Cleint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MinioServices : IMinioServices
    {
        public async Task CreateBucket(IMinioClient minio,
            string bucketName = "my-bucket-name", string loc = "us-east-1")
        {
            if (minio is null) throw new ArgumentNullException(nameof(minio));

            try
            {
                Console.WriteLine("Running example for API: MakeBucketAsync");
                await minio.MakeBucketAsync(
                    new MakeBucketArgs()
                        .WithBucket(bucketName)
                        .WithLocation(loc)
                ).ConfigureAwait(false);
                Console.WriteLine($"Created bucket {bucketName}");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
            }
        }
    }
}
