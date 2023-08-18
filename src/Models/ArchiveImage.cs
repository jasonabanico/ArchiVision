using Azure.AI.Vision.ImageAnalysis;

namespace ArchiVision.Models
{
    public class ArchiveImage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public List<string> RephrasedCaptions { get; set; }
        public string Description { get; set; } // generated text
        public ContentTags Tags { get; set; }
        public List<string> TagSynonyms { get; set; }
    }
}
