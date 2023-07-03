using Cleint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface  IMinioServices
    {
        Task CreateBucket(IMinioClient minio, string bucketName = "my-bucket-name", string loc = "us-east-1");
    }
}
