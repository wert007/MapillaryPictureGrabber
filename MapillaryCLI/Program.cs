using Mapillary.Api;
using Mapillary.Api.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapillaryCLI
{
	class Program
	{
		static void Main(string[] args)
		{
			//TODO:
			// CleanUp I: Upload to github
			// UI + Timer: So that you can use it as an alternative to Wally
			// CleanUp II: Learn to read a JSON....
			string clientId;
			using (var fs = new FileStream(@".\client-id.txt", FileMode.Open))
			using (var reader = new StreamReader(fs))
			{
				clientId = reader.ReadToEnd();
			}
			var closeTo = new Coordinates(126.979101f, 37.567304f);

			var min = new Coordinates(-16.347656f, 19.890723f);
			var max = new Coordinates(49.042969f, -28.381735f);
			var box = new BoundingBox(min, max);
			//closeTo = box.RandomPoint();

			var result = Requester.SearchImage(clientId, box, 10000, Requester.MaxPages);
			var random = new Random();
			var feature = result.Features.ElementAt(random.Next(result.Features.Count()));
			var image = feature.Result;
			ImageConverter.foo(image);
			var uri = new Uri(image.GetFile(ImageSize.ExtraLarge));
			Wallpaper.Set(uri, Wallpaper.Style.Fill);
			Console.WriteLine("Image found & downloaded.");
			Console.Read();
		}
	}
}
