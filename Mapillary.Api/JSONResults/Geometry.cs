using Mapillary.Api.DataTypes;

namespace Mapillary.Api.JSONResults
{
	public class Geometry : MapillaryJSONResult
	{
		public Geometry(string type) : base(type)
		{
		}

		public Geometry(Coordinates coordinates, string type) : base(type)
		{
			Coordinates = coordinates;
		}

		public Coordinates Coordinates { get; set; }
	}
}
