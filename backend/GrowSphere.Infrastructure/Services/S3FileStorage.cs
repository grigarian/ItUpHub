using Amazon.S3;
using Amazon.S3.Model;
using GrowSphere.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GrowSphere.Infrastructure.Services;

public class S3FileStorage : IFileStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3FileStorage(IAmazonS3 s3Client, IConfiguration config)
    {
        _s3Client = s3Client;
        _bucketName = config["MinIO:BucketName"]!;

        InitializeBucketAsync().Wait();
    }
    
    private async Task InitializeBucketAsync()
    {
        try
        {
            var exists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
            if (!exists)
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = _bucketName,
                    UseClientRegion = true
                });
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to initialize MinIO bucket: {ex.Message}");
        }
    }
    
    public async Task<FileUploadResult> UploadFileAsync(
        Stream fileStream, 
        string filePath, 
        string contentType)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = filePath,
            InputStream = fileStream,
            ContentType = contentType
        };

        await _s3Client.PutObjectAsync(request);
        return new FileUploadResult(filePath);
    }

    public async Task DeleteFileAsync(string filePath)
    {
        await _s3Client.DeleteObjectAsync(_bucketName, filePath);
    }

    public async Task<Stream> GetFileAsync(string filePath)
    {
        var response = await _s3Client.GetObjectAsync(_bucketName, filePath);
        return response.ResponseStream;
    }
    
    private string GetContentType(string fileName)
    {
        return Path.GetExtension(fileName).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}