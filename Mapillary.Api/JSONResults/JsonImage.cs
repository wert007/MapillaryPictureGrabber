using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapillary.Api.DataTypes;

namespace Mapillary.Api.JSONResults
{
	public class JsonImage
	{ 
		public float? CameraAngle { get; set; }
		public string CameraMake { get; set; }
		public string CameraModel { get; set; }
		public DateTime CapturedAt { get; set; }
		public string Key { get; set; }
		public bool Pano { get; set; }
		public string ProjectKey { get; set; } 
		public string SequenceKey { get; set; }
		public string UserKey { get; set; }
		public string Username { get; set; }

		internal Image GenerateImage()
		{
			return new Image(CameraAngle, CameraMake, CameraModel, CapturedAt, Key, Pano, ProjectKey, SequenceKey, UserKey, Username);
		}
	}
}
