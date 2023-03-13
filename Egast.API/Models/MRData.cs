namespace Egast.API.Models
{
	public class MRData
	{
		public string Xmlns { get; set; }
		public string Series { get; set; }
		public string Url { get; set; }
		public string Limit { get; set; }
		public string Offset { get; set; }
		public string Total { get; set; }
		public RaceTable RaceTable { get; set; }
		public StandingsTable StandingsTable { get; set; }
	}
}
