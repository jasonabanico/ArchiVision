using ArchiVision.Services.Interfaces;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ArchiVision.Services
{
    public class SpeechService : ISpeechService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IChatService> _logger;

        public SpeechService(
            IConfiguration configuration,
            ILogger<IChatService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SpeakTextAsync(string text, string outputFile)
        {
            var speechKey = _configuration["SpeechKey"];
            var speechRegion = _configuration["SpeechRegion"];
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                using var result = await speechSynthesizer.SpeakSsmlAsync($"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='en-US-AriaNeural'>{text}</voice></speak>");

                if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                {
                    using var fileStream = File.Create(outputFile);
                    await fileStream.WriteAsync(result.AudioData);
                    fileStream.Close();
                }
            }
        }
    }
}
