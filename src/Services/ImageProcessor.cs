using ArchiVision.Models;
using ArchiVision.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
            await _imageAnalysisService.CaptionImageAsync(image);
            await _imageAnalysisService.TagImageAsync(image);
            await _chatService.GenerateDescriptionAsync(image);
            await _archiveFileService.Write(image);
            await _speechService.SpeakTextAsync(image.Description, $"{image.Id}.mp3");
        }
    }
}
