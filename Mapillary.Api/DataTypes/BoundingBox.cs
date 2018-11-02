using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapillary.Api.DataTypes
{
	public class BoundingBox
	{
		static Random random = new Random();
		public BoundingBox(Coordinates min, Coordinates max)
		{
			Min = min;
			Max = max;
		}

		public Coordinates Min { get; }
		public Coordinates Max { get; }

		public override string ToString()
		{
			return Min.ToString() + "," + Max.ToString();
		}

		public Coordinates RandomPoint()
		{
			float percentLongitude= (float)random.NextDouble();
			float percentLattitude = (float)random.NextDouble();
			return new Coordinates(Min.Longitude + (Max.Longitude - Min.Longitude) * percentLongitude,
											Min.Lattitude + (Max.Lattitude - Min.Lattitude) * percentLattitude);
		}
	}
}
