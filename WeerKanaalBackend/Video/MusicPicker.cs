namespace WeerKanaalBackend.Video;

public static class MusicPicker
{
    private const string MusicDir = "Music";
    private static readonly string[] AudioExtensions = [".mp3", ".wav", ".m4a", ".ogg", ".aac"];
    
    public static string PickRandom()
    {
        var tracks = Directory.EnumerateFiles(MusicDir)
            .Where(f => AudioExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
            .ToArray();

        if (tracks.Length == 0)
            throw new InvalidOperationException($"No audio tracks found in '{MusicDir}'.");

        return tracks[Random.Shared.Next(tracks.Length)];
    }
}
