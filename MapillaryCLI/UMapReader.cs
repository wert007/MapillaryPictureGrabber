using Mapillary.Api.DataTypes;
using Mapillary.Api.JSONResults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapillaryCLI
{
	public class UMapObject
	{
		public string Name { get; set; }
		public string Description { get; set; }
	}

	public class UMapReader
	{
		public static FeatureCollection<UMapObject> Read(string map)
		{
			FeatureCollection<UMapObject> jsonResult = null;
			Feature<UMapObject> featureToAdd = null;
			UMapObject personToAdd = null;
			Geometry geometryToAdd = null;
			using (JsonTextReader reader = new JsonTextReader(new StringReader(map)))
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
											jsonResult = new FeatureCollection<UMapObject>((string)reader.Value);
											break;
										case "Feature":
											if (personToAdd != null)
												featureToAdd.Result = personToAdd;
											if (geometryToAdd != null)
												featureToAdd.Geometry = geometryToAdd;
											if (featureToAdd != null)
												jsonResult.Add(featureToAdd);
											featureToAdd = new Feature<UMapObject>((string)reader.Value);
											break;
										case "Point":
											if (geometryToAdd != null)
												featureToAdd.Geometry = geometryToAdd;
											geometryToAdd = new Geometry((string)reader.Value);
											break;
										default:
											Console.WriteLine($"Unhandled Type ({(string)reader.Value})");
											break;
									}
									break;
								case "features":
									break;
								case "properties":
									if (personToAdd != null)
										featureToAdd.Result = personToAdd;
									personToAdd = new UMapObject();
									break;
								case "name":
									reader.Read();
									personToAdd.Name = (string)reader.Value;
									break;
								case "description":
									reader.Read();
									personToAdd.Description = (string)reader.Value;
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
									Console.WriteLine($"Unhandled Property ({reader.Value}) occured.");
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
							Console.WriteLine($"Type {reader.TokenType} occured. Value is {reader.Value}.");
							break;
							#endregion
					}
				}
			}


			if (personToAdd != null)
				featureToAdd.Result = personToAdd;
			if (geometryToAdd != null)
				featureToAdd.Geometry = geometryToAdd;
			if (featureToAdd != null)
				jsonResult.Add(featureToAdd);

			return jsonResult;
		}
	}
}
