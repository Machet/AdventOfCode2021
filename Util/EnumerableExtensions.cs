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
}
