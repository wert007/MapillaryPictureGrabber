using System.Collections.Generic;

namespace Mapillary.Api.JSONResults
{
	public class FeatureCollection<T> : MapillaryJSONResult
	{
		public FeatureCollection(string type) : base(type)
		{
			_features = new List<Feature<T>>();
		}

		private List<Feature<T>> _features;
		public IEnumerable<Feature<T>> Features => _features;

		internal void Add(Feature<T> featureToAdd)
		{
			_features.Add(featureToAdd);
		}
	}
}
