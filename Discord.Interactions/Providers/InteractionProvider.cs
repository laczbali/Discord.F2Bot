using Discord.Core;
using Discord.Core.Models;
using Discord.Core.Utils;
using Egast.API;

namespace Discord.F2.Providers
{
    public class InteractionProvider : DiscordInteractionsBase
    {
		private readonly DiscordClientConfig config;
		private readonly ErgastAPI ergastAPI;

		public InteractionProvider(
            DiscordClientConfig config,
            ErgastAPI ergastAPI) : base(config)
        {
			this.config = config;
			this.ergastAPI = ergastAPI;
		}

        public override ApplicationCommand[] GlobalCommands => new ApplicationCommand[]
        {
            new ApplicationCommand(
                "f1-help",
				"Shows the available commands of the bot",
                (Interaction interaction) =>
                {
                    var commandInfos = GlobalCommands.Select(c => $"`/{c.Name}`\n    {c.Description}");
					var help = string.Join("\n\n", commandInfos);
                    var info = "Made by blaczko - https://github.com/laczbali/Discord.F2Bot";

					return Task.FromResult($"{help}\n\n{info}");
                }),

            new ApplicationCommand(
                "f1-next",
                "Shows general info on the upcoming race",
                async (Interaction interaction) =>
                {
                    var nextRace = await this.ergastAPI.GetNextRace();
                    var nextRaceDateTime = TimeZoneInfo.ConvertTimeFromUtc(nextRace.DateTime, this.config.TargetTimezone);

					var response = $"The next event is **{nextRace.RaceName}** on **{nextRaceDateTime.ToString("yyyy.MM.dd")}** at **{nextRaceDateTime:HH:mm}**\n({(nextRace.DateTime-DateTime.UtcNow).TotalDays:0} days from today)";

                    return response;
				}),

            new ApplicationCommand(
                "f1-quali",
                "Shows the qualifying results of the upcoming race",
                async (Interaction interaction) =>
                {
                    var nextEvent = await this.ergastAPI.GetNextRace();
                    var nextRaceWithResults = await this.ergastAPI.GetRaceResults(nextEvent.Round);

                    string qualiString = "No results";
					if(nextRaceWithResults.QualifyingResults != null)
                    {
						qualiString = string.Join("\n",
						    nextRaceWithResults.QualifyingResults.Select(q => $"{q.Position}. {q.Driver.GivenName} {q.Driver.FamilyName}"));
					}

                    string sprintString = "No results";
					if(nextRaceWithResults.SprintResults != null)
                    {
						sprintString = string.Join("\n",
						    nextRaceWithResults.SprintResults.Select(q => $"{q.Position}. {q.Driver.GivenName} {q.Driver.FamilyName}"));
					}

					var response = $"**Quali and Sprint results of the {nextEvent.RaceName}**\n\n";
                    response += $"Qualifying - {nextRaceWithResults.Qualifying.DateTime:yyyy.MM.dd - HH:mm}\n```{qualiString}```\n\n";
					response += $"Sprint - {nextRaceWithResults.Sprint?.DateTime.ToString("yyyy.MM.dd - HH:mm") ?? "No sprint for this event"}\n```{sprintString}```\n\n";

					return response;
                }),

            new ApplicationCommand(
                "f1-last",
                "Shows ALL results of the last race (in spoiler tags)",
                async (Interaction interaction) =>
                {
                    var lastEvent = await this.ergastAPI.GetLastRace();
                    var lastEventWithResults = await this.ergastAPI.GetRaceResults(lastEvent.Round);

                    string resultString = string.Empty;
                    if(lastEventWithResults.Results != null)
                    {
                        resultString = string.Join("\n",
                            lastEventWithResults.Results.Select(q =>
                            {
                                var pos = q.IsDnf ? "DNF" : $"{q.Position.PadLeft(2, '0')}.";
                                return $"{pos} {q.Driver.GivenName} {q.Driver.FamilyName}";
                            }));
					}

                    var response = $"**Race results of the {lastEvent.RaceName}**\n||```{resultString}```||";

                    return response;
				}),

            new ApplicationCommand(
                "f1-schedule",
                "Shows the schedule of the current season",
                async (Interaction interaction) =>
                {
					var nextEvent = await this.ergastAPI.GetNextRace();
					var schedule = await this.ergastAPI.GetSchedule();
                    var raceStrings = schedule.Select(race =>
                    {
                        var convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(race.DateTime, this.config.TargetTimezone);
                        return $"{convertedDateTime:yyyy.MM.dd - HH:mm} - {race.RaceName}";
                    });

                    var response = string.Join("\n", raceStrings);

                    response = $"**The full schedule of the current season is**\n```" + response + "```";
                    response = response + "\n" + $"The next race is {(nextEvent.DateTime-DateTime.UtcNow).TotalDays:0} days from now";

                    return response;
                })
        };
    }
}
