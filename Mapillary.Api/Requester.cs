using Mapillary.Api.DataTypes;
using Mapillary.Api.JSONResults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mapillary.Api
{
	public static class Requester
	{
		public static readonly int MaxPages = 1000;

		public static FeatureCollection<Image> SearchImage(string clientId, BoundingBox bbox, int radius = 100, int per_page = 200)
		{
			return SearchImage(clientId, bbox, null, null, null, null, null, per_page, null, radius, null, null, null, null);
		}
		public static FeatureCollection<Image> SearchImage(string clientId, Coordinates closeTo, int radius = 200, int per_page = 200)
		{
			return SearchImage(clientId, null, closeTo, null, null, null, null, per_page, null, radius, null, null, null, null);
		}
		public static FeatureCollection<Image> SearchImage(string clientId, Coordinates closeTo, Coordinates lookAt, int radius = 200, int per_page = 200)
		{
			return SearchImage(clientId, null, closeTo, null, null, lookAt, null, per_page, null, radius, null, null, null, null);
		}
		/// <summary>
		/// The response is a FeatureCollection object with a list of image features ordered by captured_at by default. If closeto is provided, image features will be ordered by their distances to the closeto location.
		/// </summary>
		/// <param name="bbox">Filter by the bounding box, given as minx,miny,maxx,maxy.</param>
		/// <param name="closeTo">Filter by a location that images are close to, given as longitude,latitude.</param>
		/// <param name="endTime">Filter images that are captured before end_time.</param>
		/// <param name="imageKeys">Filter images by a list of image keys.</param>
		/// <param name="lookAt">Filter images that images are taken in the direction of the specified location (and therefore that location is likely to be visible in the images), given as longitude,latitude. Note that If lookat is provided without geospatial filters like closeto or bbox, then it will search global images that look at the point.</param>
		/// <param name="pano">Filer panoramic images (true) or flat images (false).</param>
		/// <param name="perPage">The number of images per page (default 200, and maximum 1000).</param>
		/// <param name="projectKeys">Filter images by projects, given as project keys.</param>
		/// <param name="radius">Filter images within the radius around the closeto location (default 100 meters).</param>
		/// <param name="sequenceKeys">Filter images by sequences.</param>
		/// <param name="startTime">Filter images that are captured since start_time.</param>
		/// <param name="userkeys">Filter images captured by users, given as user keys.</param>
		/// <param name="usernames">Filter images captured by users, given as usernames.</param>
		/// <returns>The response is a FeatureCollection object with a list of image features ordered by captured_at by default. </returns>
		public static FeatureCollection<Image> SearchImage(string cliendId, BoundingBox bbox, Coordinates closeTo, DateTime? endTime, string[] imageKeys, Coordinates lookAt, bool? pano, int? perPage, string[] projectKeys,
			int? radius, string[] sequenceKeys, DateTime? startTime, string[] userkeys, string[] usernames)
		{
			
			FeatureCollection<Image> jsonResult = null;
			JsonImage imageToAdd = null;
			Feature<Image> featureToAdd = null;
			Geometry geometryToAdd = null;
			var data = RequestData(cliendId, bbox, closeTo, endTime, imageKeys, lookAt, pano, perPage, projectKeys, radius, sequenceKeys, startTime, userkeys, usernames);
			using (JsonTextReader reader = new JsonTextReader(data))
			{
				while (reader.Read())
				{
					switch (reader.TokenType)
					{
						case JsonToken.PropertyName:
							switch ((string)reader.Value)
							{
								case "type":
									reader.Read();
									switch ((string)reader.Value)
									{
										case "FeatureCollection":
											jsonResult = new FeatureCollection<Image>((string)reader.Value);
											break;
										case "Feature":
											if (imageToAdd != null)
												featureToAdd.Result = imageToAdd.GenerateImage();
											if (geometryToAdd != null)
												featureToAdd.Geometry = geometryToAdd;
											if (featureToAdd != null)
												jsonResult.Add(featureToAdd);
											featureToAdd = new Feature<Image>((string)reader.Value);
											break;
										case "Point":
											if (geometryToAdd != null)
												featureToAdd.Geometry = geometryToAdd;
											geometryToAdd = new Geometry((string)reader.Value);
											break;
										default:
										//	Console.WriteLine($"Unhandled Type ({(string)reader.Value})");
											break;
									}
									break;
								case "features":
									break;
								case "properties":
									if (imageToAdd != null)
										featureToAdd.Result = imageToAdd.GenerateImage();
									imageToAdd = new JsonImage();
									break;
								case "ca":
									reader.Read();
									imageToAdd.CameraAngle = float.Parse(reader.Value.ToString());
									break;
								case "camera_make":
									reader.Read();
									imageToAdd.CameraMake = (string)reader.Value;
									break;
								case "camera_model":
									reader.Read();
									imageToAdd.CameraModel = (string)reader.Value;
									break;
								case "captured_at":
									reader.Read();
									imageToAdd.CapturedAt = DateTime.Parse(reader.Value.ToString());
									break;
								case "key":
									reader.Read();
									imageToAdd.Key = (string)reader.Value;
									break;
								case "pano":
									reader.Read();
									imageToAdd.Pano = (bool)reader.Value;
									break;
								case "sequence_key":
									reader.Read();
									imageToAdd.SequenceKey = (string)reader.Value;
									break;
								case "user_key":
									reader.Read();
									imageToAdd.UserKey = (string)reader.Value;
									break;
								case "username":
									reader.Read();
									imageToAdd.Username = (string)reader.Value;
									break;
								case "geometry":
									break;
								case "coordinates":
									while ((reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Integer) && reader.Read()) ;
									float longitude = float.Parse(reader.Value.ToString());
									reader.Read();
									float lattitude = float.Parse(reader.Value.ToString());
									geometryToAdd.Coordinates = new Coordinates(longitude, lattitude);
									break;
								default:
									//Console.WriteLine($"Unhandled Property ({reader.Value}) occured.");
									break;
							}
							break;
						#region unused
						case JsonToken.EndObject:
						case JsonToken.EndArray:
						case JsonToken.EndConstructor:
						case JsonToken.StartObject:
						case JsonToken.StartArray:
						case JsonToken.StartConstructor:
							//silencee
							break;
						case JsonToken.Date:
						case JsonToken.Boolean:
						case JsonToken.String:
						case JsonToken.Float:
						case JsonToken.Integer:
						case JsonToken.Raw:
						case JsonToken.Null:
						case JsonToken.Comment:
						case JsonToken.Undefined:
						case JsonToken.Bytes:
						case JsonToken.None:
						default:
						//	Console.WriteLine($"Type {reader.TokenType} occured. Value is {reader.Value}.");
							break;
							#endregion
					}
				}
			}

			if (imageToAdd != null)
				featureToAdd.Result = imageToAdd.GenerateImage();
			if (geometryToAdd != null)
				featureToAdd.Geometry = geometryToAdd;
			if (featureToAdd != null)
				jsonResult.Add(featureToAdd);

			return jsonResult;
		}

		private static StringReader RequestData(string cliendId, BoundingBox bbox, Coordinates closeTo, DateTime? endTime, string[] imageKeys, Coordinates lookAt, bool? pano, int? perPage, string[] projectKeys,
			int? radius, string[] sequenceKeys, DateTime? startTime, string[] userkeys, string[] usernames)
		{
			//https://a.mapillary.com/v3/images/?closeto=13.0006076843,55.6089295863&radius=100&per_page=10000000&client_id=TG1sUUxGQlBiYWx2V05NM0pQNUVMQTo2NTU3NTBiNTk1NzM1Y2U2
			var requestString = new StringBuilder();
			requestString.Append("https://a.mapillary.com/v3/images/");
			if (cliendId != null)
				requestString.Append("?client_id=" + cliendId);
			else throw new AccessViolationException("Without a client_id you may not request data from this api.");
			if (bbox != null)
				requestString.Append("&bbox=" + bbox.ToString());
			if (closeTo != null)
				requestString.Append("&closeto=" + closeTo.ToString());
			if (endTime.HasValue)
				requestString.Append("&end_time=" + endTime.Value.ToString("YYYY-MM-DD"));
			if (imageKeys != null && imageKeys.Length > 0)
				requestString.Append("&image_keys=" + string.Join(",", imageKeys));
			if (lookAt != null)
				requestString.Append("&lookat=" + lookAt.ToString());
			if (pano.HasValue)
				requestString.Append("&pano=" + pano.Value.ToString());
			if (perPage.HasValue)
				requestString.Append("&per_page=" + perPage.Value.ToString());
			if (projectKeys != null && projectKeys.Length > 0)
				requestString.Append("&project_keys=" + string.Join(",", projectKeys));
			if (radius.HasValue)
				requestString.Append("&radius=" + radius.Value.ToString());
			if (sequenceKeys != null && sequenceKeys.Length > 0)
				requestString.Append("&seqeuence_keys=" + string.Join(",", sequenceKeys));
			if (startTime.HasValue)
				requestString.Append("&start_time=" + startTime.Value.ToString("YYYY-MM-DD"));
			if (userkeys != null && userkeys.Length > 0)
				requestString.Append("&userkeys=" + string.Join(",", userkeys));
			if (usernames != null && usernames.Length > 0)
				requestString.Append("&usernames=" + string.Join(",", usernames));
			var client = new HttpClient();
			var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestString.ToString());
			var result = client.SendAsync(requestMessage).Result.Content.ReadAsStringAsync().Result;
			using (FileStream fs = new FileStream(@"C:\Users\Wert007\Desktop\dev-mapillary-test.json", FileMode.Create))
			using (StreamWriter writer = new StreamWriter(fs))
				writer.Write(result);
			return new StringReader(result);
		}
	}
}
