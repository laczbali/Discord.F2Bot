namespace Egast.API.Models
{
	public class RaceResult
	{
		public string Number { get; set; }
		public string Position { get; set; }
		public string PositionText { get; set; }
		public string Points { get; set; }
		public Driver Driver { get; set; }
		public Constructor Constructor { get; set; }
		public string Grid { get; set; }
		public string Laps { get; set; }
		public string Status { get; set; }
		public EventTime Time { get; set; }
		public FastestLap FastestLap { get; set; }

		public bool IsDnf
		{
			get
			{
				if (Status == "Finished") return false;
				if (Status.StartsWith("+")) return false;
				return true;
			}
		}
	}
}
