using System.Collections.Generic;

namespace Egast.API.Models
{
	public class StandingsTable
	{
		public string Season { get; set; }
		public string Round { get; set; }
		public List<StandingsList> StandingsLists { get; set; }
	}
}
