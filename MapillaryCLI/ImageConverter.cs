using Mapillary.Api.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace MapillaryCLI
{
	public static class ImageConverter
	{
		public static void SaveImageTo(Image image, string target)
		{
			var imagePath = image.GetFile(ImageSize.ExtraLarge);
			using (WebClient client = new WebClient())
			{
				try
				{
					client.DownloadFile(new Uri(imagePath), target);
				}
				catch { }
			}
		}
	}
}
