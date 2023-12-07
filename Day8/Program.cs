var vals = new List<int> { 2, 4, 3, 7 };
var inputs = File.ReadAllLines("input.txt")
	.Select(x => x.Split(" | "))
	.Select(x => new { Patterns = x[0].Split(" "), Values = x[1].Split(" ").ToList() })
	.ToList();

Console.WriteLine("Part 1: " + inputs.SelectMany(x => x.Values).Where(x => vals.Contains(x.Length)).Count());
Console.WriteLine("Part 2: " + inputs.Select(x => GetValue(x.Patterns, x.Values)).Sum());

int GetValue(ICollection<string> patterns, IList<string> values)
{
	patterns = patterns.Select(p => new string(p.OrderBy(x => x).ToArray())).ToList();
	values = values.Select(p => new string(p.OrderBy(x => x).ToArray())).ToList();
	var one = patterns.First(x => x.Length == 2);
	var seven = patterns.First(x => x.Length == 3);
	var four = patterns.First(x => x.Length == 4);
	var eight = patterns.First(x => x.Length == 7);
	var nine = patterns.Where(x => x.Length == 6 && HasCommonSegments(x, four, 4)).Single();
	var six = patterns.Where(x => x.Length == 6 && HasCommonSegments(x, one, 1)).Single();
	var zero = patterns.Where(x => x.Length == 6 && x != six && x != nine).Single();
	var three = patterns.Where(x => x.Length == 5 && HasCommonSegments(x, one, 2)).Single();
	var two = patterns.Where(x => x.Length == 5 && HasCommonSegments(x, four, 2) && HasCommonSegments(x, one, 1)).Single();
	var five = patterns.Where(x => x.Length == 5 && HasCommonSegments(x, four, 3) && HasCommonSegments(x, one, 1)).Single();

	var map = new Dictionary<string, int>
	{
		[zero] = 0,
		[one] = 1,
		[two] = 2,
		[three] = 3,
		[four] = 4,
		[five] = 5,
		[six] = 6,
		[seven] = 7,
		[eight] = 8,
		[nine] = 9,
	};

	return map[values[0]] * 1000 + map[values[1]] * 100 + map[values[2]] * 10 + map[values[3]];
}

bool HasCommonSegments(string val1, string val2, int count)
{
	return val1.Intersect(val2).Count() == count;
}