using System.Collections.Immutable;

var connections = File.ReadAllLines("input.txt")
	.Select(x => x.Split("-"))
	.SelectMany(x => GetConnections(x[0], x[1]))
	.GroupBy(x => x.from)
	.ToDictionary(x => x.Key, x => x.Select(y => y.to).ToHashSet());

var count = GeneratePossibleNodes("start", "", ImmutableHashSet<Visit>.Empty).Count();
Console.WriteLine(count);

IEnumerable<string> GeneratePossibleNodes(string node, string path, ImmutableHashSet<Visit> visitedNodes)
{
	if (node == "start" && visitedNodes.Any())
	{
		yield break;
	}

	if (node == "end")
	{
		yield return path + " end ";
		yield break;
	}

	if (!char.IsUpper(node[0]) && visitedNodes.Any(n => n.count == 2) && visitedNodes.Contains(new Visit(node, 1)))
	{
		yield break;
	}

	var newNodes = char.IsUpper(node[0])
		? visitedNodes
		: visitedNodes.Contains(new Visit(node, 1))
			? visitedNodes.Add(new Visit(node, 2))
			: visitedNodes.Add(new Visit(node, 1));

	foreach (var connected in connections[node])
	{
		foreach (var subPath in GeneratePossibleNodes(connected, path + " " + node, newNodes))
		{
			yield return subPath;
		}
	}
}

IEnumerable<Connection> GetConnections(string v1, string v2)
{
	yield return new Connection(v1, v2);
	yield return new Connection(v2, v1);
}

record Connection(string from, string to);
record Visit(string node, int count);