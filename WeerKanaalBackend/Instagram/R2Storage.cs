using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace WeerKanaalBackend.Instagram;

public class R2Storage
{
    private readonly IAmazonS3 _client;

    public R2Storage()
    {
        var accountId = Env("R2_ACCOUNT_ID");
        _client = new AmazonS3Client(
            Env("R2_ACCESS_KEY_ID"),
            Env("R2_SECRET_ACCESS_KEY"),
            new AmazonS3Config
            {
                ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                AuthenticationRegion = "auto",
                ForcePathStyle = true,
                // R2 doesn't implement the AWS SDK v4 default streaming trailer checksum,
                // so only add checksums when the operation actually requires them.
                RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
                ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED,
            });
    }

    public Task UploadFileAsync(string bucket, string key, string filePath, string contentType) =>
        _client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = bucket, Key = key, FilePath = filePath, ContentType = contentType,
            // R2 doesn't support streaming SigV4 — send a single, unsigned payload instead.
            DisablePayloadSigning = true, UseChunkEncoding = false,
        });

    public Task DeleteAsync(string bucket, string key) => _client.DeleteObjectAsync(bucket, key);

    public async Task<string?> TryGetTextAsync(string bucket, string key)
    {
        try
        {
            using var resp = await _client.GetObjectAsync(bucket, key);
            using var reader = new StreamReader(resp.ResponseStream);
            return (await reader.ReadToEndAsync()).Trim();
        }
        catch (AmazonS3Exception e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public Task PutTextAsync(string bucket, string key, string text) =>
        _client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = bucket, Key = key, ContentBody = text, ContentType = "text/plain",
            DisablePayloadSigning = true, UseChunkEncoding = false,
        });

    private static string Env(string name) =>
        Environment.GetEnvironmentVariable(name)
        ?? throw new InvalidOperationException($"Missing required environment variable: {name}");
}
