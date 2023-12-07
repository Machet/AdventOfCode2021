using Util;

var input = File.ReadAllLines("input.txt")
	.ToArray2D()
	.Map(c => int.Parse(c.ToString()));

var result1 = input.Select((x, y, val) => new { Value = val, IsMin = IsMin(x, y, input) })
	.Where(x => x.IsMin)
	.Select(x => x.Value + 1)
	.Sum();

Console.WriteLine(result1);

var result2 = input.Select((x, y, val) => new { X = x, Y = y, IsMin = IsMin(x, y, input) })
	.Where(x => x.IsMin)
	.Select(x => FindBasinSize(x.X, x.Y, input))
	.OrderByDescending(x => x)
	.Take(3)
	.Aggregate(1, (x, y) => x * y);

Console.WriteLine(result2);

int FindBasinSize(int x, int y, int[,] input)
{
	var points = new HashSet<(int x, int y)>();
	var queue = new Queue<(int x, int y)>();
	queue.Enqueue((x, y));
	points.Add((x, y));

	while (queue.Any())
	{
		var point = queue.Dequeue();
		foreach (var neighbour in input.GetNeighbourCells(point.x, point.y))
		{
			if (neighbour.value == 9 || !points.Add((neighbour.x, neighbour.y)))
			{
				continue;
			}

			queue.Enqueue((neighbour.x, neighbour.y));
		}
	}

	return points.Count;
}

bool IsMin(int x, int y, int[,] input)
{
	return input[x, y] < input.GetCloseNeighbours(x, y).Min();
}