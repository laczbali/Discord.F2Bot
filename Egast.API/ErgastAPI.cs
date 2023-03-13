using Egast.API.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Egast.API
{
	public class ErgastAPI
	{
		private readonly ILogger<ErgastAPI> logger;

		private const string CurrentSeason = "current";
		private Uri BaseUri;

		public ErgastAPI(ILogger<ErgastAPI> logger, ErgastAPIConfig apiConfig = null)
		{
			var config = apiConfig ?? new ErgastAPIConfig();
			this.BaseUri = new Uri(config.ApiBaseUrl);
			this.logger = logger;
		}

		public async Task<IEnumerable<Race>> GetSchedule(string season = CurrentSeason)
		{
			return (await this.MakeResponse(season)).RaceTable.Races;
		}

		public async Task<Race> GetNextRace()
		{
			var schedule = await this.GetSchedule();
			return schedule.FirstOrDefault(race => race.DateTime > DateTime.UtcNow);
		}

		public async Task<Race> GetLastRace()
		{
			var schedule = (await this.GetSchedule()).Reverse();
			return schedule.FirstOrDefault(race => race.DateTime < DateTime.UtcNow);
		}

		public async Task<IEnumerable<DriverStanding>> GetCurrentDriverStandings()
		{
			var standings = (await this.MakeResponse($"{CurrentSeason}/driverStandings")).StandingsTable?.StandingsLists?.FirstOrDefault()?.DriverStandings;
			if (standings == null)
			{
				this.logger?.LogWarning("Driver standings returned null");
			}

			return standings;
		}

		public async Task<IEnumerable<ConstructorStanding>> GetCurrentConstuctorStandings()
		{
			var standings = (await this.MakeResponse($"{CurrentSeason}/constructorStandings")).StandingsTable?.StandingsLists?.FirstOrDefault()?.ConstructorStandings;
			if (standings == null)
			{
				this.logger?.LogWarning("Constuctor standings returned null");
			}

			return standings;
		}

		public async Task<Race> GetRaceResults(string round, string season = CurrentSeason)
		{
			var raceList = await this.GetSchedule(season);
			var race = raceList.FirstOrDefault(r => r.Round == round);
			if(race == null)
			{
				throw new ArgumentException($"Failed to find race #{round} in the {season} season");
			}

			var qualiResults = (await this.MakeResponse($"{race.Season}/{race.Round}/qualifying")).RaceTable.Races.FirstOrDefault() ?? new Race();
			race.QualifyingResults = qualiResults.QualifyingResults;

			var sprintResults = (await this.MakeResponse($"{race.Season}/{race.Round}/sprint")).RaceTable.Races.FirstOrDefault() ?? new Race();
			race.SprintResults = sprintResults.SprintResults;

			var raceResults = (await this.MakeResponse($"{race.Season}/{race.Round}/results")).RaceTable.Races.FirstOrDefault() ?? new Race();
			race.Results = raceResults.Results;

			return race;
		}

		private async Task<MRData> MakeResponse(string relativeEndpoint)
		{
			string result;
			try
			{
				var message = new HttpRequestMessage(HttpMethod.Get, new Uri(this.BaseUri, $"{relativeEndpoint}.json"));
				var client = new HttpClient();
				result = await (await client.SendAsync(message)).Content.ReadAsStringAsync();
			}
			catch (Exception e)
			{
				this.logger?.LogError(e, "Failed to query API");
				throw;
			}

			Root responseData;
			try
			{
				responseData = JsonConvert.DeserializeObject<Root>(result);
			}
			catch (Exception e)
			{
				this.logger?.LogError(e, "Failed to deserialize response");
				throw;
			}

			return responseData.MRData;
		}
	}
}
