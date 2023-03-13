using System.Collections.Generic;

namespace Egast.API.Models
{
	public class StandingsList
	{
		public string Season { get; set; }
		public string Round { get; set; }
		public List<DriverStanding> DriverStandings { get; set; }
		public List<ConstructorStanding> ConstructorStandings { get; set; }
	}
}
