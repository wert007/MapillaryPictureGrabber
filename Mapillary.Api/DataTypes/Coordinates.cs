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

		public double DistanceTo(Coordinates other)
		{
			double f = 1.0 / 298.257223563;
			double a = 6378.137;
			double F = 0.5 * (Lattitude + other.Lattitude);
			double G = 0.5 * (Lattitude - other.Lattitude);
			double l = 0.5 * (Longitude - other.Longitude);
			double S = Math.Pow(Math.Sin(G), 2) * Math.Pow(Math.Cos(l), 2) + Math.Pow(Math.Cos(F), 2) * Math.Pow(Math.Sin(l), 2);
			double C = Math.Pow(Math.Cos(G), 2) * Math.Pow(Math.Cos(l), 2) + Math.Pow(Math.Sin(F), 2) * Math.Pow(Math.Sin(l), 2);
			double w = Math.Atan(Math.Sqrt(S / C));
			double D = 2 * w * a;
			double T = Math.Sqrt(S * C) / w;
			double H1 = (3 * T - 1) / (2 * C);
			double H2 = (3 * T + 1) / (2 * S);
			double s = D * (1 + f * H1 * Math.Pow(Math.Sin(F), 2)) * Math.Pow(Math.Cos(G), 2) - f * H2 * Math.Pow(Math.Cos(F), 2) * Math.Pow(Math.Sin(G), 2);
			return s;
		}
	}
}
