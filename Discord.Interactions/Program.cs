using Discord.Core;
using Discord.Core.Utils;
using Discord.F2.Providers;
using Egast.API;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Set up environment variable config provider
const string EnvVarPrefix = "discord_f2_";
builder.Configuration.AddEnvironmentVariables(EnvVarPrefix);
string EnvVar(string key) => builder?.Configuration.GetValue<string>(EnvVarPrefix + key) ?? throw new Exception($"Env var with key [{key}] not found");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddSingleton(conf => new DiscordClientConfig
{
	ApplicationId = EnvVar("ApplicationId"),
	ApiBaseUrl = EnvVar("ApiBaseUrl"),
	BotToken = EnvVar("BotToken"),
	ClientId = EnvVar("ClientId"),
	PublicKey = EnvVar("PublicKey"),
	TargetTimezone = TimeZoneInfo.FindSystemTimeZoneById(EnvVar("TargetTimezone"))
});

builder.Services.AddSingleton<ErgastAPI>();
builder.Services.AddSingleton<DiscordInteractionsBase, InteractionProvider>();
builder.Services.AddSingleton<DiscordRequestHandler>();

// Set up app
var app = builder.Build();
if(!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}
app.MapControllers();

// Perform startup tasks
var discordInteractions = app.Services.GetRequiredService<DiscordInteractionsBase>();
_ = Task.Run(async () =>
{
	try
	{
		await discordInteractions.RegisterGlobalCommands();
	}
	catch(Exception e)
	{
		Debug.WriteLine("Startup error: " + e.Message);
	}
});

// Done
app.Run();
