using Azure.AI.Vision.ImageAnalysis;

namespace ArchiVision.Models
{
    public class ArchiveImage
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public List<string> RephrasedCaptions { get; set; } = new List<string>();
        public string Description { get; set; } // generated text
        public ContentTags ContentTags { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> TagSynonyms { get; set; } = new List<string>();
        public Dictionary<string, string> RawOutput { get; set; } = new Dictionary<string, string>();
    }
}
