using System.Collections.Generic;

namespace Egast.API.Models
{
	public class RaceTable
	{
		public string Season { get; set; }
		public List<Race> Races { get; set; }
	}
}
