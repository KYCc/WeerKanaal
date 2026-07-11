using System.Diagnostics;

namespace WeerKanaalBackend.Video;

public class VideoEncoder
{
    public async Task<string> ToMp4Async(string webmPath, string musicPath)
    {
        Directory.CreateDirectory("Instagram");
        var tomorrow = DateTime.Today.AddDays(1);
        var mp4Path = Path.Combine("Instagram", $"reel-{tomorrow:yyyy-MM-dd}.mp4");

        var psi = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            RedirectStandardError = true,
            UseShellExecute = false,
            ArgumentList =
            {
                "-y",
                "-ss", "0.5",              // drop the first 0.5s (white flash before React paints)
                "-i", webmPath,
                "-stream_loop", "-1", "-i", musicPath, // loop the track to cover the whole video
                "-map", "0:v:0", "-map", "1:a:0",      // video from input 0, audio from input 1
                "-c:v", "libx264",
                "-crf", "20",
                "-preset", "veryfast",
                "-pix_fmt", "yuv420p",     // required for broad/Instagram playback
                "-r", "30",                // normalize fps
                "-c:a", "aac", "-b:a", "128k",
                "-af", "afade=t=out:st=18:d=2", // 2s fade-out (tied to the ~20s cycle)
                "-shortest",               // stop at the video length
                "-movflags", "+faststart", // moov atom up front
                mp4Path,
            },
        };

        using var proc = Process.Start(psi)
            ?? throw new InvalidOperationException("Failed to start ffmpeg. Is it installed and on PATH?");

        // Read stderr before waiting so a full pipe buffer can't deadlock ffmpeg.
        var log = await proc.StandardError.ReadToEndAsync();
        await proc.WaitForExitAsync();
        if (proc.ExitCode != 0)
            throw new InvalidOperationException($"ffmpeg exited with code {proc.ExitCode}:\n{log}");

        return mp4Path;
    }
}
