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
		public static void foo(Image image)
		{
			var imagePath = image.GetFile(ImageSize.ExtraLarge);
			using (WebClient client = new WebClient())
			{
				client.DownloadFile(new Uri(imagePath), @"C:\Users\Wert007\Desktop\dev-mapillary-image.jpeg");
			}
		}
	}
}
