# ArchiVision - AI Extended Metadata + AI Assistive Technology for Archive Images

ArchiVision is an AI Extended Metadata and AI Assistive Technology system that harnesses a variety of AI technologies to generate extended metadata and assistive descriptions from the image itself.

It is powered by the precision of Azure AI's Computer Vision, the expressiveness of Open AI's Large Language Models, and the articulation of Azure AI's Speech Services.

GovHack Hackerspace: https://hackerspace.govhack.org/projects/archivision

## Demo
https://bit.ly/archivision

## Setup
In src folder, copy appsettings.json.setup to appsettings.json, and update the following keys using instruction in the links:
- AZURE_AI_SERVICES_VISION_ENDPOINT: https://learn.microsoft.com/en-us/rest/api/computer-vision/
- AZURE_AI_SERVICES_VISION_KEY: https://learn.microsoft.com/en-us/rest/api/computer-vision/
- OPENAI_API_KEY: https://www.guidingtech.com/how-to-generate-openai-api-key/
- AZURE_AI_SERVICES_SPEECH_KEY: https://learn.microsoft.com/en-us/azure/ai-services/speech-service/get-started-speech-to-text

## Run
- Using command or terminal, go to src folder.
- Update archive.csv file with your input image id, title, and urls.
- Ensure you have dotnet. If not, download dotnet at https://dotnet.microsoft.com/en-us/download.
- dotnet build
- dotnet run
- The outputs are generated as *.json and *.mp3 files, named after the id in the csv document.
