using ArchiVision.Models;
using ArchiVision.Services.Interfaces;
using Azure;
using Azure.AI.Vision.Common;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ArchiVision.Services
{
    public class ImageAnalysisService : IImageAnalysisService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IImageAnalysisService> _logger;
        private readonly VisionServiceOptions _visionServiceOptions;

        public ImageAnalysisService(IConfiguration configuration, ILogger<IImageAnalysisService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var visionServiceEndpoint = _configuration["VisionServiceEndpoint"];
            var visionServiceKey = _configuration["VisionServiceKey"];
            _visionServiceOptions = new VisionServiceOptions(
                visionServiceEndpoint,
                new AzureKeyCredential(visionServiceKey));
        }

        public async Task CaptionImageAsync(ArchiveImage image)
        {
            using var imageSource = VisionSource.FromUrl(new Uri(image.Url));
            var analysisOptions = new ImageAnalysisOptions()
            {
                Features = ImageAnalysisFeature.Caption,
                GenderNeutralCaption = true
            };
            using var analyzer = new ImageAnalyzer(_visionServiceOptions, imageSource, analysisOptions);
            var tcsEventReceived = new TaskCompletionSource<bool>();
            var result = await analyzer.AnalyzeAsync();

            if (result.Reason != ImageAnalysisResultReason.Analyzed)
            {
                Console.WriteLine(" Analysis failed.");
                var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
                Console.WriteLine($"   Error reason : {errorDetails.Reason}");
                Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
                Console.WriteLine($"   Error message: {errorDetails.Message}");
            }

            image.Caption = result.Caption.Content;
        }

        public async Task TagImageAsync(ArchiveImage image)
        {
            using var imageSource = VisionSource.FromUrl(new Uri(image.Url));
            var analysisOptions = new ImageAnalysisOptions()
            {
                Features = ImageAnalysisFeature.Tags,
                GenderNeutralCaption = true
            };
            using var analyzer = new ImageAnalyzer(_visionServiceOptions, imageSource, analysisOptions);
            var tcsEventReceived = new TaskCompletionSource<bool>();
            var result = await analyzer.AnalyzeAsync();

            if (result.Reason != ImageAnalysisResultReason.Analyzed)
            {
                Console.WriteLine(" Analysis failed.");
                var errorDetails = ImageAnalysisErrorDetails.FromResult(result);
                Console.WriteLine($"   Error reason : {errorDetails.Reason}");
                Console.WriteLine($"   Error code : {errorDetails.ErrorCode}");
                Console.WriteLine($"   Error message: {errorDetails.Message}");
            }

            image.ContentTags = result.Tags;
            foreach (var contentTag in  image.ContentTags)
            {
                // can add filtering here based on confidence
                image.Tags.Add(contentTag.Name);
            }
        }
    }
}
