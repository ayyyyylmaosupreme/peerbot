ad: will make anything for you at 30$/h or fixed price quotes

first open source project give feedback and no bully

WTFPL license

wip bot might clean it and dockerize it later


might make ui for changing configs later

Documentation


ChatBot/HivescoreFetcher -> change to ur steam id

HivescoreLogger.cs -> change file path

MessageLogger -> change file path

MessageSpeaker.cs -> change file path

MessageSpeakerSettingsManager -> change file path

ChatBot/Properties/launchSettings:
{
  "profiles": {
    "ChatBot": {
      "commandName": "Project",
      "environmentVariables": {
        "GOOGLE_APPLICATION_CREDENTIALS ": "google how to use google voice authentication"
      }
    }
  }


note to self: this is probably useless but it might not be
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "TwitchDetails": {
        "BotUsername": "ai_under_the_stars",
        "ChannelUsername": "peerless_under_the_stars",
        "ClientID": 
        "Secret": 
    }
}

YoutubeClient/Program.cs -> get ur own auth


Add ur google voice auth here
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:54549",
      "sslPort": 44381
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "environmentVariables": {
        "KeyGOOGLE_APPLICATION_CREDENTIALS ": "C:\\Users\\Peer\\Desktop\\peerbot-329501-7bffcbd28a99.json",
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "YoutubeClient": {
      "commandName": "Project",
      "launchBrowser": true,
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:5001;http://localhost:5000"
    }
  }
}
