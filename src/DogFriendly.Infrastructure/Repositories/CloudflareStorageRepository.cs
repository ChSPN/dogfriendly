using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using DogFriendly.Domain.Options;
using DogFriendly.Domain.Repositories;
using Microsoft.Extensions.Options;
using System.Net;

namespace DogFriendly.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for Cloudflare storage.
    /// </summary>
    public class CloudflareStorageRepository : IFileStorageRepository
    {
        private readonly string _bucketName;
        private readonly AmazonS3Client _s3Client;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudflareStorageRepository"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public CloudflareStorageRepository(IOptions<FileStorageOption> options)
        {
            _bucketName = options.Value.BucketName;
            var config = new AmazonS3Config
            {
                ServiceURL = $"https://{options.Value.AccountId}.eu.r2.cloudflarestorage.com",
                ForcePathStyle = true,
                LogResponse = true,
                LogMetrics = true
            };
            var credentials = new BasicAWSCredentials(options.Value.AccessKey, options.Value.SecretKey);
            _s3Client = new AmazonS3Client(credentials, config);
        }

        /// <summary>
        /// Uploads the file asynchronous.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContent">Content of the file.</param>
        /// <returns></returns>
        public async Task<bool> UploadFileAsync(string fileName, byte[] fileContent)
        {
            using (var stream = new MemoryStream(fileContent))
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = stream,
                    DisablePayloadSigning = true
                };
                var response = await _s3Client.PutObjectAsync(putRequest);
                return response.HttpStatusCode == HttpStatusCode.OK;
            }
        }

        /// <summary>
        /// Removes the file asynchronous.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public async Task<bool> RemoveFileAsync(string fileName)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };
            var response = await _s3Client.DeleteObjectAsync(deleteRequest);
            return response.HttpStatusCode == HttpStatusCode.NoContent;
        }
    }
}
