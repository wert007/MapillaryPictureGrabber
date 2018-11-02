using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapillary.Api.DataTypes
{
	public sealed class Image
	{
		/// <summary>
		///   Image's camera angle in [0, 360) degrees. (optional)
		/// </summary>
		public float? CameraAngle { get; }

		/// <summary>
		///   Camera make.
		/// </summary>
		public string CameraMake { get; }

		/// <summary>
		///   Camera model.
		/// </summary>
		public string CameraModel { get; }

		/// <summary>
		///   When image was captured.
		/// </summary>
		public DateTime CapturedAt { get; }

		/// <summary>
		///   Image key.
		/// </summary>
		public string Key { get; }//Has own Type at the mapillary docs.

		/// <summary>
		///  Whether the image is panorama (true), or not (false).
		/// </summary>
		public bool Pano { get; }

		/// <summary>
		///   Which project the image belongs to. Absent if it doesn't belong to any project. (optional)
		/// </summary>
		public string ProjectKey { get; } //See Key-Property

		/// <summary>
		///   Which sequence the image belongs to.
		/// </summary>
		public string SequenceKey { get; } //See Key-Property

		/// <summary>
		///   User who captured the image.
		/// </summary>
		public string UserKey { get; } //See Key-Property

		/// <summary>
		///  Username of who captured the image.
		/// </summary>
		public string Username { get; }

		private Image()
		{

		}

		internal Image(float? cameraAngle, string cameraMake, string cameraModel, DateTime capturedAt, string key, bool pano, string projectKey, string sequenceKey, string userKey, string username)
		{
			CameraAngle = cameraAngle;
			CameraMake = cameraMake;
			CameraModel = cameraModel;
			CapturedAt = capturedAt;
			Key = key;
			Pano = pano;
			ProjectKey = projectKey;
			SequenceKey = sequenceKey;
			UserKey = userKey;
			Username = username;
		}

		public string GetFile(ImageSize size)
		{
			string strSize = string.Empty;
			switch (size)
			{
				case ImageSize.Small:
					strSize = "thumb-320.jpg";
					break;
				case ImageSize.Medium:
					strSize = "thumb-640.jpg";
					break;
				case ImageSize.Large:
					strSize = "thumb-1024.jpg";
					break;
				case ImageSize.ExtraLarge:
					strSize = "thumb-2048.jpg";
					break;
				default:
					break;
			}
			return "https://d1cuyjsrcm0gby.cloudfront.net/" + $"{Key}/{strSize}";
		}
	}

	public enum ImageSize
	{
		Small,
		Medium,
		Large,
		ExtraLarge
	}

}
