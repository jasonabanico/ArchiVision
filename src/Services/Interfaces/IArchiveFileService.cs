using ArchiVision.Models;

namespace ArchiVision.Services.Interfaces
{
    public interface IArchiveFileService
    {
        void Load(string filePath);
        void Write(ArchiveImage image);
    }
}
