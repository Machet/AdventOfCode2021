var input = File.ReadAllLines("input.txt")
	.ToList();

var points = input.TakeWhile(i => !string.IsNullOrEmpty(i))
	.Select(i => i.Split(","))
	.Select(i => new Point(int.Parse(i[0]), int.Parse(i[1])))
	.ToHashSet();

var folds = input.SkipWhile(i => !string.IsNullOrEmpty(i))
	.Skip(1)
	.Select(i => i.Replace("fold along ", string.Empty))
	.Select(i => i.Split("="))
	.Select(i => new Fold(i[0], int.Parse(i[1])))
	.ToList();

foreach (var fold in folds)
{
	points = PerformFold(points, fold);
	Console.WriteLine("Fold " + points.Count);
}

var maxX = points.Select(p => p.x).Max();
var maxY = points.Select(p => p.y).Max();

for (int y = 0; y <= maxY; y++)
{
	Console.WriteLine();

	for (int x = 0; x <= maxX; x++)
	{
		Console.Write(points.Contains(new Point(x, y)) ? "#" : " ");
	}
}

HashSet<Point> PerformFold(HashSet<Point> points, Fold fold)
{
	return points.Select(p =>
	{
		if (fold.axis == "x")
		{
			return new Point(p.x < fold.pos ? p.x : fold.pos - (p.x - fold.pos), p.y);
		}
		else
		{
			return new Point(p.x, p.y < fold.pos ? p.y : fold.pos - (p.y - fold.pos));
		}
	}).ToHashSet();
}

record Point(int x, int y);
record Fold(string axis, int pos);
