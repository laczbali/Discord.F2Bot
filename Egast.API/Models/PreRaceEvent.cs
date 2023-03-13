using System;

namespace Egast.API.Models
{
	public class PreRaceEvent
	{
		public string Date { get; set; }
		public string Time { get; set; }

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
