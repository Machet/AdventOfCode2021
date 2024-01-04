var input = File.ReadAllLines("input.txt")
	.ToList();

var cost1 = new int[input.First().Length, input.Count];
var cost2 = new int[input.First().Length * 5, input.Count * 5];

for (int y = 0; y < input.Count; y++)
{
	for (int x = 0; x < input.Count; x++)
	{
		int val = int.Parse(input[y][x].ToString());
		cost1[x, y] = val;

		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				cost2[i * input.Count + x, j * input.Count + y] = Wrap(val + i + j);
			}
		}
	}
}

var map1 = new Map<int>(cost1);
var map2 = new Map<int>(cost2);

Console.WriteLine(GetShortestPath(map1));
Console.WriteLine(GetShortestPath(map2));


static int GetShortestPath(Map<int> map1)
{
	var queue = new PriorityQueue<MapItem<int>, int>();
	var item = new MapItem<int>(new Point(0, 0), 0);
	var end = new Point(map1.width - 1, map1.height - 1);
	var visited = new Dictionary<Point, int>();
	visited[item.point] = 0;

	while (item != null)
	{
		if (item.point == end)
		{
			return item.value;
		}

		foreach (var neighbour in map1.GetNeighbourItems(item.point))
		{
			var len = item.value + neighbour.value;

			if (!visited.ContainsKey(neighbour.point) || visited[neighbour.point] > len)
			{
				visited[neighbour.point] = len;
				queue.Enqueue(new MapItem<int>(neighbour.point, len), item.value + neighbour.value + end.Dist(neighbour.point));
			}
		}

		item = queue.Dequeue();
	}

	return 0;
}

int Wrap(int v)
{
	if (v < 10) return v;
	if (v >= 10 && v < 19) return v - 9;
	return v - 18;
}

record Point(int x, int y)
{
	internal int Dist(Point point) => Math.Abs(point.x - x) + Math.Abs(point.y - y);
}

record MapItem<T>(Point point, T value);
record Map<T>(T[,] items)
{
	public int width => items.GetLength(0);
	public int height => items.GetLength(1);

	public bool IsWithin(Point point)
		=> point.x >= 0 && point.x < width && point.y >= 0 && point.y < height;

	public IEnumerable<Point> GetNeighbours(Point point)
	{
		var neighbours = new List<Point>()
		{
			new Point(point.x - 1, point.y),
			new Point(point.x + 1, point.y),
			new Point(point.x, point.y - 1),
			new Point(point.x, point.y + 1)
		};

		return neighbours.Where(p => IsWithin(p));
	}

	public MapItem<T> GetItem(Point point)
		=> new MapItem<T>(point, items[point.x, point.y]);

	public IEnumerable<MapItem<T>> GetNeighbourItems(Point point)
		=> GetNeighbours(point).Select(p => new MapItem<T>(p, items[p.x, p.y]));
}