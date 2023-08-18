namespace ArchiVision.Services.Interfaces
{
    public interface ISpeechService
    {
        Task SpeakTextAsync(string text, string outputFile);
    }
}
