namespace Mapillary.Api.JSONResults
{
	public class Feature<T> : MapillaryJSONResult
	{
		public Feature(string type) : base(type) { }
		public Feature(T result, Geometry geometry, string type): base(type)
		{
			Result = result;
			Geometry = geometry;
		}

		public T Result { get; set; }
		public Geometry Geometry { get; set; }
	}
}
