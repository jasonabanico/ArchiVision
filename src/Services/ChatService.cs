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
            var message = $"rephrase this message 10 times, separate into lines. no bullets or numbers. '{image.Caption}'";
            var response = await ChatAsync(message);
            var rephrasedCaptions = response.Split('\n');
            image.RephrasedCaptions = rephrasedCaptions.ToList();
        }

        public async Task GenerateTagSynonymsAsync(ArchiveImage image)
        {
            var tags = string.Join(", ", image.Tags);
            var message = $"generate synonyms of these tags: {tags}, and output comma delimited list enclosed in []";
            var response = await ChatAsync(message);
            var pattern = @"\[(.*?)\]";
            var match = Regex.Match(response, pattern);
            var group = match.Groups[0];
            var capture = group.Captures[0];
            var synonymTags = capture.Value.Split(',');
            image.TagSynonyms = synonymTags.ToList();
        }

        public async Task GenerateDescriptionAsync(ArchiveImage image)
        {
            var tags = string.Empty;
            var message = $"turn these into a simple 2 to 3 sentences to describe an image: '{image.Title}', '{image.Caption}', ";
            foreach (var tag in image.Tags)
            {
                // NOTE: Can add tag.Confidence filter here to only use tags with confidence above a certain level
                if (!string.IsNullOrEmpty(tags))
                    tags += ", ";
                tags += tag.Name.Trim();
            }
            image.Description = await ChatAsync(message + tags);
        }
    }
}
