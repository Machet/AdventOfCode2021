namespace Util;

public static class EnumerableExtensions
{
	public static string JoinStrings(this IEnumerable<string> strings, string separator = "") => string.Join(separator, strings);

	public static IEnumerable<T> TakeUntilPlus<T>(this IEnumerable<T> items, Func<T, bool> predicate)
	{
		foreach (var item in items)
		{
			yield return item;
			if (predicate(item))
				break;
		}
	}

	public static IEnumerable<(T first, T second)> GenerateAllCombinations<T>(this IEnumerable<T> items)
	{
		var toIterate = items.ToList();
		for (int i = 0; i < toIterate.Count; i++)
		{
			for (var j = i + 1; j < toIterate.Count; j++)
			{
				yield return (toIterate[i], toIterate[j]);
			}
		}
	}
}
