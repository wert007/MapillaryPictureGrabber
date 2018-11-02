using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapillary.Api.DataTypes
{
	public class Coordinates
	{
		public Coordinates(float longitude, float lattitude)
		{
			Longitude = longitude;
			Lattitude = lattitude;
		}

		//What Datatype to use? See here: https://stackoverflow.com/a/25120203
		public float Longitude { get; }
		public float Lattitude { get; }

		public override string ToString()
		{
			var usCulture = CultureInfo.CreateSpecificCulture("us-US");
			return Longitude.ToString(usCulture) + "," + Lattitude.ToString(usCulture);
		}
	}
}
