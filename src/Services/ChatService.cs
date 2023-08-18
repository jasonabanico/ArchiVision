using ArchiVision.Models;
using ArchiVision.Services.Interfaces;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ArchiVision.Services
{
    public class ChatService : IChatService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IChatService> _logger;

        public ChatService(IConfiguration configuration,
            ILogger<IChatService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private async Task<string> ChatAsync(string prompt)
        {
            var openAiApiKey = _configuration["OpenAiApiKey"];
            var client = new OpenAIClient(openAiApiKey);

            var chatCompletionsOptions = new ChatCompletionsOptions();
            chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, prompt));

            string modelName = "gpt-3.5-turbo";
            var completionsResponse = await client.GetChatCompletionsAsync(modelName, chatCompletionsOptions);
            return completionsResponse.Value.Choices[0].Message.Content;
        }

        public async Task GenerateRephrasedCaptionsAsync(ArchiveImage image)
        {
            var message = $"generate csv of 10 rephrases in double quotes: '{image.Caption}'";
            var response = await ChatAsync(message);
            var rephrasedCaptions = response.Split('\n');
            foreach (var rephrasedCaption in rephrasedCaptions)
            {
                if (rephrasedCaption.Contains('"'))
                {
                    var snippets = rephrasedCaption.Split(",");
                    foreach (var snippet in snippets)
                        image.RephrasedCaptions.Add(snippet.Replace("\"", ""));
                }
            }
        }

        public async Task GenerateTagSynonymsAsync(ArchiveImage image)
        {
            var tags = string.Join(", ", image.Tags);
            var message = $"{tags}: Generate 10 synonyms for each tag in CSV in double quotes";
            var response = await ChatAsync(message);
            var responseLines = response.Split('\n');
            foreach (var responseLine in responseLines)
            {
                if (responseLine.Contains('"'))
                {
                    var snippets = responseLine.Split(",");
                    foreach (var snippet in snippets)
                    {
                        var tagSynonym = snippet.Replace("\"", "");
                        if (!image.TagSynonyms.Contains(tagSynonym))
                            image.TagSynonyms.Add(tagSynonym.Trim());
                    }
                }
            }
        }

        public async Task GenerateDescriptionAsync(ArchiveImage image)
        {
            var tags = string.Empty;
            var message = $"turn these into a simple 2 to 3 sentences to describe an image: '{image.Title}', '{image.Caption}',";
            message += string.Join(",", image.Tags);
            image.Description = await ChatAsync(message + tags);
        }
    }
}
