using Util;

var input = File.ReadAllLines("input.txt")
	.ToArray2D()
	.Map(x => int.Parse(x.ToString()));

var result1 = 0;

for (int i = 1; ; i++)
{
	var flashed = new HashSet<Point>();
	input = input.Map(i => i + 1);
	var toFlash = input.ToCells().Where(c => c.value > 9);

	while (toFlash.Any())
	{
		foreach (var item in toFlash)
		{
			flashed.Add(item.Point);

			foreach (var neighbour in input.GetAllNeighbourCells(item.x, item.y))
			{
				input[neighbour.x, neighbour.y] += 1;
			};
		}

		toFlash = input.ToCells().Where(c => c.value > 9 && !flashed.Contains(c.Point));
	}

	input = input.Map(x => x <= 9 ? x : 0);

	if (i <= 100)
	{
		result1 += flashed.Count;
	}

	if (input.Select(x => x).All(x => x == 0))
	{
		Console.WriteLine("Result 1: " + result1);
		Console.WriteLine("Result 2: " + i);
		break;
	}
}

