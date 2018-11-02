using Mapillary.Api;
using Mapillary.Api.DataTypes;
using Mapillary.Api.JSONResults;
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
			FeatureCollection<UMapObject> persons;
			using (var fs = new FileStream(@".\abi_18.json", FileMode.Open))
			using (var reader = new StreamReader(fs))
			{
				persons = UMapReader.Read(reader.ReadToEnd());
			}
			string target = @"C:\Users\Wert007\Desktop\Abiturienten3\";
			if(!Directory.Exists(target))
				Directory.CreateDirectory(target);
			foreach (var personFeature in persons.Features)
			{
				var personImages = Requester.SearchImage(clientId, null, personFeature.Geometry.Coordinates, 100000, 1);
				if(personImages.Features.Count() <= 0 || personFeature.Result.Name == null)
				{
					//Console.WriteLine($"Poor {personFeature.Result.Name}, there are no pictures in a radius of 100km..");
					continue;
				}
				var imageFeature = personImages.Features.First();
				var personImage = imageFeature.Result;
				var name = personFeature.Result.Name;
				var path = Path.Combine(target, name) + ".jpeg";
				Console.WriteLine($"Derivation for {name}: {imageFeature.Geometry.Coordinates.DistanceTo(personFeature.Geometry.Coordinates)}km");
				ImageConverter.SaveImageTo(personImage, path);
			}
				var closeTo = new Coordinates(126.979101f, 37.567304f);

			var min = new Coordinates(-16.347656f, 19.890723f);
			var max = new Coordinates(49.042969f, -28.381735f);
			var box = new BoundingBox(min, max);

			var images = Requester.SearchImage(clientId, box, 10000, Requester.MaxPages);
			var random = new Random();
			var feature = images.Features.ElementAt(random.Next(images.Features.Count()));
			var image = feature.Result;
			var imageUri = new Uri(image.GetFile(ImageSize.ExtraLarge));
			Wallpaper.Set(imageUri, Wallpaper.Style.Fill);
			Console.WriteLine("Image found & downloaded.");
			Console.Read();
		}
	}
}
