using Amazon.S3;
using Amazon.S3.Model;
using Convocatorias.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convocatorias.Infraestructure.Storage
{
    public class S3DocumentoStorageService : IDocumentoStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public S3DocumentoStorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"]!;
        }

        public async Task DeleteAsync(string fileUrl, CancellationToken ct = default)
        {
            // Extrae el key de la URL
            var uri = new Uri(fileUrl);
            var key = uri.AbsolutePath.TrimStart('/');

            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request, ct);
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, CancellationToken ct = default)
        {
            var key = $"documentos/{Guid.NewGuid()}_{fileName}";
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType
                // Si el bucket es privado, los objetos heredan esa configuración
            };
            var response = await _s3Client.PutObjectAsync(request, ct);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"https://{_bucketName}.s3.amazonaws.com/{key}";
            }
            else
            {
                throw new Exception("Error uploading file to S3");
            }
        }   
    }
}
