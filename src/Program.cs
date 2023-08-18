using ArchiVision.Services.Interfaces;
using ArchiVision.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArchiVision
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection();

            var serviceProvider = serviceCollection
                .AddLogging()
                .AddSingleton(config)
                .AddSingleton<IImageAnalysisService, ImageAnalysisService>()
                .AddSingleton<IChatService, ChatService>()
                .AddSingleton<ISpeechService, SpeechService>()
                .AddSingleton<IImageProcessor, ImageProcessor>()
                .BuildServiceProvider();

            serviceProvider
                .GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                        .CreateLogger<Program>();

            var imageProcessor = serviceProvider.GetService<IImageProcessor>();

            var archiveFileService = new ArchiveFileService();
            archiveFileService.Load("archive-act.csv");
            foreach (var archiveImage in archiveFileService.Images)
            {
                await imageProcessor.ProcessImageAsync(archiveImage);
            }
        }
    }
}