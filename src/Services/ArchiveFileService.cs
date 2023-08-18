using ArchiVision.Models;
using ArchiVision.Services.Interfaces;
using System.Text.Json;

namespace ArchiVision.Services
{
    public class ArchiveFileService : IArchiveFileService
    {
        public List<ArchiveImage> Images { get; set; } = new List<ArchiveImage>();

        public void Load(string fileName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split(',');
                        if (fields.Length >= 2)
                        {
                            ArchiveImage image = new ArchiveImage
                            {
                                Id = fields[0].Replace("\"", ""),
                                Title = fields[1].Replace("\"", ""),
                                Url = fields[2].Replace("\"", "")
                            };
                            Images.Add(image);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public void Write(ArchiveImage image)
        {
            string jsonString = JsonSerializer.Serialize(image, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Write the JSON to the specified file
            File.WriteAllText($"{image.Id}.json", jsonString);
        }
    }
}
