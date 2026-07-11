using System.Text.Json;

namespace WeerKanaalBackend.Instagram;

// host mp4 on R2 -> create container -> poll status -> media_publish -> delete -> refresh token.
public class InstagramPublisher
{
    private const string Graph = "https://graph.instagram.com/v25.0";
    private const string RefreshUrl = "https://graph.instagram.com/refresh_access_token";
    private const string TokenObject = "ig-token.txt";

    private readonly HttpClient _http = new();
    private readonly R2Storage _r2 = new();
    private readonly string _igUserId = Env("IG_USER_ID");
    private readonly string _videoBucket = Env("R2_BUCKET");
    private readonly string _tokenBucket = Env("R2_TOKEN_BUCKET");
    private readonly string _publicBaseUrl = Env("R2_PUBLIC_BASE_URL").TrimEnd('/');

    public async Task<string> PublishReelAsync(string mp4Path, string caption)
    {
        var token = await GetTokenAsync();
        var key = Path.GetFileName(mp4Path);

        await _r2.UploadFileAsync(_videoBucket, key, mp4Path, "video/mp4");
        var videoUrl = $"{_publicBaseUrl}/{key}";
        try
        {
            var containerId = await CreateContainerAsync(videoUrl, caption, token);
            await WaitUntilFinishedAsync(containerId, token);
            var mediaId = await PublishAsync(containerId, token);
            await RefreshTokenAsync(token); // extend the 60-day token; persist the new value
            return mediaId;
        }
        finally
        {
            await _r2.DeleteAsync(_videoBucket, key);
        }
    }

    private async Task<string> CreateContainerAsync(string videoUrl, string caption, string token)
    {
        var resp = await _http.PostAsync($"{Graph}/{_igUserId}/media",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["media_type"] = "REELS",
                ["video_url"] = videoUrl,
                ["caption"] = caption,
                ["thumb_offset"] = "2000", // cover frame at 2s, after the cards have faded in
                ["access_token"] = token,
            }));
        return await ReadIdAsync(resp, "create container");
    }

    private async Task WaitUntilFinishedAsync(string containerId, string token)
    {
        for (var attempt = 0; attempt < 30; attempt++) // poll ~2.5 min max
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            var resp = await _http.GetAsync($"{Graph}/{containerId}?fields=status_code&access_token={token}");
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
                throw new InvalidOperationException($"Status check failed ({(int)resp.StatusCode}): {body}");

            var status = JsonDocument.Parse(body).RootElement.GetProperty("status_code").GetString();
            switch (status)
            {
                case "FINISHED": return;
                case "ERROR":
                case "EXPIRED":
                    throw new InvalidOperationException($"Container {containerId} failed with status {status}.");
            }
        }
        throw new TimeoutException($"Container {containerId} was not FINISHED in time.");
    }

    private async Task<string> PublishAsync(string containerId, string token)
    {
        var resp = await _http.PostAsync($"{Graph}/{_igUserId}/media_publish",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["creation_id"] = containerId,
                ["access_token"] = token,
            }));
        return await ReadIdAsync(resp, "publish");
    }

    private async Task RefreshTokenAsync(string token)
    {
        var resp = await _http.GetAsync($"{RefreshUrl}?grant_type=ig_refresh_token&access_token={token}");
        if (!resp.IsSuccessStatusCode) return;
        var body = await resp.Content.ReadAsStringAsync();
        if (JsonDocument.Parse(body).RootElement.TryGetProperty("access_token", out var t)
            && t.GetString() is { Length: > 0 } newToken)
        {
            await _r2.PutTextAsync(_tokenBucket, TokenObject, newToken);
        }
    }

    private async Task<string> GetTokenAsync() =>
        await _r2.TryGetTextAsync(_tokenBucket, TokenObject) ?? Env("IG_ACCESS_TOKEN");

    private static async Task<string> ReadIdAsync(HttpResponseMessage resp, string step)
    {
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode)
            throw new InvalidOperationException($"Instagram {step} failed ({(int)resp.StatusCode}): {body}");
        return JsonDocument.Parse(body).RootElement.GetProperty("id").GetString()!;
    }

    private static string Env(string name) =>
        Environment.GetEnvironmentVariable(name)
        ?? throw new InvalidOperationException($"Missing required environment variable: {name}");
}
