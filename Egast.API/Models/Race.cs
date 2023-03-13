using System;
using System.Collections.Generic;

namespace Egast.API.Models
{
	public class Race
	{
		public string Season { get; set; }
		public string Round { get; set; }
		public string Url { get; set; }
		public string RaceName { get; set; }
		public Circuit Circuit { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public PreRaceEvent FirstPractice { get; set; }
		public PreRaceEvent SecondPractice { get; set; }
		public PreRaceEvent ThirdPractice { get; set; }
		public PreRaceEvent Qualifying { get; set; }
		public PreRaceEvent Sprint { get; set; }
		public List<QualifyingResult> QualifyingResults { get; set; }
		public List<SprintResult> SprintResults { get; set; }
		public List<RaceResult> Results { get; set; }

		public DateTime DateTime
		{
			get
			{
				var splitDate = this.Date.Split('-');
				var splitTime = this.Time.Replace("Z", string.Empty).Split(':');

				var dateTime = new DateTime(
					int.Parse(splitDate[0]),
					int.Parse(splitDate[1]),
					int.Parse(splitDate[2]),
					int.Parse(splitTime[0]),
					int.Parse(splitTime[1]),
					int.Parse(splitTime[2]),
					DateTimeKind.Utc);

				return dateTime;
			}
		}
	}
}
