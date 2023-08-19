# ArchiVision

## Demo
- ACT: https://bit.ly/archivision-act

## Setup
In src folder, copy appsettings.json.setup to appsettings.json, and update the following keys using instruction in the links:
- AZURE_AI_SERVICES_VISION_ENDPOINT: https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/quickstarts-sdk/image-analysis-client-library-40
- AZURE_AI_SERVICES_VISION_KEY: https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/quickstarts-sdk/image-analysis-client-library-40
- OPENAI_API_KEY: https://platform.openai.com/
- AZURE_AI_SERVICES_SPEECH_KEY: https://learn.microsoft.com/en-us/azure/ai-services/speech-service/get-started-text-to-speech

## Run
- Using command or terminal, go to src folder.
- Update archive.csv file with your input image id, title, and urls.
- Ensure you have dotnet. If not, download dotnet at https://dotnet.microsoft.com/en-us/download.
- dotnet build
- dotnet run
- The outputs are generated as *.json and *.mp3 files, named after the id in the csv document.