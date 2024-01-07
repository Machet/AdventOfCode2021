using System.Collections.Immutable;

var input = File.ReadAllLines("input.txt")
	.ToList();

var template = input.First();

var insertions = input
	.Skip(2)
	.Select(i => i.Split(" -> "))
	.ToDictionary(i => i[0], i => i[1][0]);


var result1 = template.GroupBy(c => c).ToImmutableDictionary(t => t.Key, t => (long)t.Count());
var result2 = template.GroupBy(c => c).ToImmutableDictionary(t => t.Key, t => (long)t.Count());
var cache = new Dictionary<(char, char, int), ImmutableDictionary<char, long>>();

for (int i = 0; i < template.Length - 1; i++)
{
	result1 = CombineDictionaries(result1, GetElementCounts(template[i], template[i + 1], 10, insertions));
	result2 = CombineDictionaries(result2, GetElementCounts(template[i], template[i + 1], 40, insertions));
}

Console.WriteLine("1: " + (result1.Select(r => r.Value).Max() - result1.Select(r => r.Value).Min()));
Console.WriteLine("2: " + (result2.Select(r => r.Value).Max() - result2.Select(r => r.Value).Min()));

ImmutableDictionary<char, long> GetElementCounts(char first, char second, int iterations, Dictionary<string, char> insertions)
{
	var key = (first, second, iterations);
	if (cache.TryGetValue(key, out var cacheResult)) return cacheResult;

	var insertion = insertions[first.ToString() + second];
	var iteratoionValue = ImmutableDictionary<char, long>.Empty.Add(insertion, 1);
	if (iterations == 1)
	{
		return iteratoionValue;
	}

	var leftResult = GetElementCounts(first, insertion, iterations - 1, insertions);
	var rightResult = GetElementCounts(insertion, second, iterations - 1, insertions);

	var result = CombineDictionaries(iteratoionValue, CombineDictionaries(leftResult, rightResult));
	cache[key] = result;
	return result;
}

static ImmutableDictionary<char, long> CombineDictionaries(ImmutableDictionary<char, long> leftResult, ImmutableDictionary<char, long> rightResult)
{
	var result = ImmutableDictionary.CreateBuilder<char, long>();
	foreach (var key in leftResult.Keys.Union(rightResult.Keys))
	{
		leftResult.TryGetValue(key, out long left);
		rightResult.TryGetValue(key, out long right);
		result.Add(key, left + right);
	}

	return result.ToImmutable();
}