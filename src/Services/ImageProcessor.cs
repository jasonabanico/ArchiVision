using ArchiVision.Models;
using ArchiVision.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ArchiVision.Services
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IImageAnalysisService _imageAnalysisService;
        private readonly IChatService _chatService;
        private readonly ISpeechService _speechService;
        private readonly IArchiveFileService _archiveFileService;
        private readonly ILogger<IImageProcessor> _logger;

        public ImageProcessor(
            IImageAnalysisService imageAnalysisService,
            IChatService chatService,
            ISpeechService speechService,
            IArchiveFileService archiveFileService,
            ILogger<IImageProcessor> logger)
        {
            _imageAnalysisService = imageAnalysisService;
            _chatService = chatService;
            _speechService = speechService;
            _archiveFileService = archiveFileService;
            _logger = logger;
        }

        public async Task ProcessImageAsync(ArchiveImage image)
        {
            Console.WriteLine($"Processing image: {image.Id}");
            Console.WriteLine($"Title: {image.Title}");
            Console.WriteLine($"URL: {image.Url}");
            await _imageAnalysisService.CaptionImageAsync(image);
            Console.WriteLine($"Caption: {image.Caption}");
            await _chatService.GenerateRephrasedCaptionsAsync(image);
            Console.WriteLine($"Rephrased Captions:");
            Console.WriteLine($"{string.Join('\n', image.RephrasedCaptions)}");
            await _imageAnalysisService.TagImageAsync(image);
            Console.WriteLine($"Tags: {string.Join(",",image.Tags)}");
            await _chatService.GenerateTagSynonymsAsync(image);
            Console.WriteLine($"Tag Synonyms: {string.Join(",", image.TagSynonyms)}");
            await _chatService.GenerateDescriptionAsync(image);
            Console.WriteLine($"Description: {image.Description}");
            await _speechService.SpeakTextAsync(image.Description, $"{image.Id}.mp3");

            string jsonString = JsonSerializer.Serialize(image, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            Console.WriteLine(jsonString);

            _archiveFileService.Write(image);
        }
    }
}
