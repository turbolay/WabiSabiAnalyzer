namespace WabiSabiAnalyzer;

public static class Extensions
{
	public static ulong Sum(this IEnumerable<ulong> me)
	{
		ulong inputSum = 0;
		foreach (var item in me)
		{
			inputSum += item;
		}
		return inputSum;
	}
	
	public static double? Median<TColl, TValue>(this IEnumerable<TColl> source, Func<TColl, TValue> selector)
	{
		return source.Select(selector).Median();
	}

	public static double? Median<T>(
		this IEnumerable<T> source)
	{
		if (Nullable.GetUnderlyingType(typeof(T)) != null)
			source = source.Where(x => x != null);

		int count = source.Count();
		if (count == 0)
			return null;

		source = source.OrderBy(n => n);

		int midpoint = count / 2;
		if (count % 2 == 0)
			return (Convert.ToDouble(source.ElementAt(midpoint - 1)) + Convert.ToDouble(source.ElementAt(midpoint))) / 2.0;
		else
			return Convert.ToDouble(source.ElementAt(midpoint));
	}

	public static IEnumerable<(TColl value, int count)> GetIndistinguishable<TColl>(this IEnumerable<TColl> source)
	{
		return source.GroupBy(x => x)
			.ToDictionary(x => x.Key, y => y.Count())
			.Select(x => (x.Key, x.Value));
	}
}