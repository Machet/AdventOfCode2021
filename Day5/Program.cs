var lines = File.ReadAllLines("input.txt")
	.Select(l => ParseLine(l))
	.ToList();

var task1 = lines
	.Where(l => l.IsHorizontal || l.IsVertical)
	.SelectMany(x => x.GetPoints())
	.GroupBy(x => x)
	.Where(x => x.Count() > 1)
	.Select(x => x.Key)
	.ToList();

var task2 = lines
	.SelectMany(x => x.GetPoints())
	.GroupBy(x => x)
	.Where(x => x.Count() > 1)
	.Select(x => x.Key)
	.ToList();

var diag = lines
	.Where(l => !l.IsHorizontal && !l.IsVertical)
	.ToList();

var diagl = lines.SelectMany(x => x.GetPoints()).ToList();

Console.WriteLine(task1.Count);
Console.WriteLine(task2.Count);

Line ParseLine(string l)
{
	var inputs = l.Split(" -> ");
	var startInput = inputs[0].Split(",");
	var endInput = inputs[1].Split(",");

	var start = new Point(int.Parse(startInput[0]), int.Parse(startInput[1]));
	var end = new Point(int.Parse(endInput[0]), int.Parse(endInput[1]));
	return new Line(start, end);
}

public record Line(Point start, Point end)
{
	public bool IsVertical => start.x == end.x;
	public bool IsHorizontal => start.y == end.y;

	public IEnumerable<Point> GetPoints()
	{
		if (IsVertical)
		{
			for (int y = Math.Min(start.y, end.y); y <= Math.Max(start.y, end.y); y++)
			{
				yield return new Point(start.x, y);
			}
		}
		else if (IsHorizontal)
		{
			for (int x = Math.Min(start.x, end.x); x <= Math.Max(start.x, end.x); x++)
			{
				yield return new Point(x, start.y);
			}
		}
		else
		{
			for (int x = start.x, y = start.y; ;)
			{
				yield return new Point(x, y);

				if (x == end.x || y == end.y)
				{
					yield break;
				}

				x = end.x > start.x ? x + 1 : x - 1;
				y = end.y > start.y ? y + 1 : y - 1;
			}
		}
	}
}

public record Point(int x, int y);