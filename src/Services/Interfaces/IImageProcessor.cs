using ArchiVision.Models;

namespace ArchiVision.Services.Interfaces
{
    public interface IImageProcessor
    {
        Task ProcessImageAsync(ArchiveImage image);
    }
}
