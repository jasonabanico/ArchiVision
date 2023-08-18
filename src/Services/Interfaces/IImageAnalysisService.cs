using ArchiVision.Models;

namespace ArchiVision.Services.Interfaces
{
    public interface IImageAnalysisService
    {
        Task CaptionImageAsync(ArchiveImage image);
        Task TagImageAsync(ArchiveImage image);
    }
}
