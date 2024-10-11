using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class Weighted<T>
{
	[Serializable]
	public class Pair
	{
		public T obj;
		public float value;
		public Pair() { }

		public Pair(T obj, float value)
		{
			this.obj = obj;
			this.value = value;
		}
	}

	public List<Pair> sortedWeights;

	public Weighted() { }

	public Weighted(IEnumerable<KeyValuePair<T, float>> values)
	{
		sortedWeights = values.OrderBy(x => x.Value)
							  .Select(x => new Pair(x.Key, x.Value))
							  .ToList();
	}

	public T GetRandomValue()
	{
		float randomValue = UnityEngine.Random.value * sortedWeights.Sum(x => x.value);

		foreach (var weight in sortedWeights)
		{
			randomValue -= weight.value;

			if (randomValue <= 0)
				return weight.obj;
		}

		return default;
	}
}
