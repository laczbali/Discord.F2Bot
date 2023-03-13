namespace Egast.API.Models
{
	public class FastestLap
	{
		public string Rank { get; set; }
		public string Lap { get; set; }
		public EventTime Time { get; set; }
		public AverageSpeed AverageSpeed { get; set; }
	}
}
