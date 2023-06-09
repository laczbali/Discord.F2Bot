﻿using System.Collections.Generic;

namespace Egast.API.Models
{
	public class DriverStanding
	{
		public string Position { get; set; }
		public string PositionText { get; set; }
		public string Points { get; set; }
		public string Wins { get; set; }
		public Driver Driver { get; set; }
		public List<Constructor> Constructors { get; set; }
	}
}
