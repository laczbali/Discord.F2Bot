# Discord.F2Bot
A new an improved version of the [old Discord bot](https://github.com/laczbali/f1-discord),
It is meant to give information of the current F1 schedule.

- It gets the F1 info from the [Ergast API](http://ergast.com/mrd/).
- For Discord functionality, it depends on [Discord.Core](https://github.com/laczbali/Discord.Core).

# Developer Guide
**Prerequisites**
- Visual Studio with web development tools installed
- Install AWS tools with `dotnet tool install -g Amazon.Lambda.Tools`
- Install the AWS CLI
- Set up AWS CLI profile with `aws configure`
  - Generate an AWS Access Key in IAM
  - Set the default region to `us-east-1`
- Create and set up a Discord Bot
  - Set it to private
  - Add the interaction endpoint (you need to publish the app first to AWS) `[AWS_URL]/interaction`
- Install and configure ngrok
- Set the following env vars in Properties\launchSettings.json
	- discord_f2_ApplicationId
	- discord_f2_ApiBaseUrl
	- discord_f2_BotToken
	- discord_f2_ClientId
	- discord_f2_PublicKey
	- discord_f2_TargetTimezone
- Download the [Discord.Core](https://github.com/laczbali/Discord.Core) repo

**Development**
- Deploy with `dotnet lambda deploy-function` from the project (not solution) directory
- If you are doing a first-time publish:
  - Set the function name to `DiscordF2Interactions`
  - Create a new IAM Role `DiscordF2Role`
  - Select IAM Policy `6 - Basic Execution`
- Start the app
- Start ngrok `ngrok http [HTTP_PORT]`
- Set the ngrok URL as the interaction endpoint URL
