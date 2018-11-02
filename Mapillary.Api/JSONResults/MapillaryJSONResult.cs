using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapillary.Api.DataTypes;

namespace Mapillary.Api.JSONResults
{
	public abstract class MapillaryJSONResult
	{
		public MapillaryJSONResult(string type)
		{
			Type = type;
		}
		public string Type { get; }

	}
}
