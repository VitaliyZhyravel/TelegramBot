PolisUA – Telegram Insurance Bot

PolisUA is a Telegram bot that allows users to obtain car insurance in just a few simple steps. The user sends photos of their passport and vehicle registration certificate. The bot reads the data using the Mindee service, confirms it with the user, informs them of a fixed insurance cost ($100), then generates a fake insurance policy in PDF format and sends it directly to the chat. The bot's responses are generated using OpenAI to ensure natural communication.

⚙️ Installation and Launch

✅ Requirements

.NET 9 SDK

Telegram Bot Token

Mindee API Key

OpenAI API Key

📁 Environment Variables

Set the following variables (e.g., in a .env file or appsettings.Production.json):

ApiKeys.TelegramBotService=your-telegram-token-here  
ApiKeys.MindeeService=your-mindee-api-key  
ApiKeys.OpenAiService=your-openai-api-key  

🚀 Launch

Using .NET CLI:

dotnet run --project TelegramBot.Api

Or using Docker:

docker build -t telegram-insurance-bot .
docker run -p 8080:10000 --env-file .env telegram-insurance-bot

To use this bot in Telegram, it must be deployed to a public server with HTTPS support (e.g., Render, Railway, or your own VPS), as Telegram sends updates via webhook.

🔄 Bot Workflow

The user sends the /start command. The bot greets the user and suggests using buttons to upload the required documents. After each document is uploaded, the bot reads the data and asks the user to confirm it. Once all documents are provided and confirmed, the user can press the "Generate Insurance" button. The bot warns that generation will cost $100, and after confirmation—if all data is complete and verified—it generates a PDF insurance file and sends it to the user.

🔗 Link

Telegram Bot: @TaskForInterviewDICEUS_bot

👤 Author

Vitalii Zhuravel

