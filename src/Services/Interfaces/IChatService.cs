using ArchiVision.Models;

namespace ArchiVision.Services.Interfaces
{
    public interface IChatService
    {
        public Task GenerateRephrasedCaptionsAsync(ArchiveImage image);
        public Task GenerateTagSynonymsAsync(ArchiveImage image);
        public Task GenerateDescriptionAsync(ArchiveImage image);
    }
}
