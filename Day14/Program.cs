var input = File.ReadAllLines("input.txt")
	.ToList();

var template = input.First().AsEnumerable();

var insertions = input
	.Skip(2)
	.Select(i => i.Split(" -> "))
	.Select(i => new Insertion(i[0][0], i[0][1], i[1][0]))
	.ToList();

var prevStats = new List<Stat>();

for (int i = 0; i < 20; i++)
{
	Console.WriteLine("Iteration " + i);

	template = PerformInsertions(template, insertions).ToList();

	var stats = template.GroupBy(t => t)
		.Select(x => new Stat(x.Key, x.LongCount()))
		.OrderByDescending(x => x.count)
		.ToList();

	foreach (var stat in stats)
	{
		var prevStat = prevStats.FirstOrDefault(s => s.c == stat.c);
		Console.WriteLine($"Char {stat.c} Count: {stat.count} Diff: {stat.count - (prevStat?.count ?? stat.count)}");
	}

	Console.WriteLine($"Result: {GetResult(stats)} Diff: {GetResult(stats) - GetResult(prevStats)}");
	prevStats = stats;
	Console.WriteLine();
}

IEnumerable<char> PerformInsertions(IEnumerable<char> template, List<Insertion> insertions)
{
	var first = template.First();
	var second = first;
	yield return first;

	foreach (var c in template.Skip(1))
	{
		second = c;
		var insertion = insertions.FirstOrDefault(i => i.first == first && i.second == second);
		if (insertion != null)
		{
			yield return insertion.toInsert;
		}

		first = second;
		yield return second;
	}
}

long GetResult(List<Stat> stats)
{
	return stats.Any() ? stats.First().count - stats.Last().count : 0;
}

record Insertion(char first, char second, char toInsert);
record Stat(char c, long count);