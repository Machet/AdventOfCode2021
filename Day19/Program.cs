var scanners = ParseScanners(File.ReadAllLines("input.txt")).ToList();

var aggregate = scanners[0];
var toProcess = new Queue<Scanner>(scanners.Skip(1));

while (toProcess.Count > 0)
{
	var scanner = toProcess.Dequeue();
	if (!AddScanner(aggregate, scanner))
	{
		toProcess.Enqueue(scanner);
	}
}

Console.WriteLine("1: " + aggregate.Vectors.Count);

static bool AddScanner(Scanner scanner, Scanner other)
{
	foreach (var point in scanner.Vectors)
	{
		var lengths = point.Value.Select(x => x.Length);

		foreach (var otherPoint in other.Vectors)
		{
			var common = otherPoint.Value.IntersectBy(lengths, x => x.Length).ToList();

			if (common.Count > 9)
			{
				var dest = point.Value.First(v => v.Length == common[0].Length);
				var source = common[0];
				var translation = GetTranslationFunction(dest, source);
				scanner.AddPoints(point.Key, otherPoint.Value.Select(translation).ToList());
				return true;
			}
		}
	}

	return false;
}

static Func<Vector, Vector> GetTranslationFunction(Vector source, Vector dest)
{
	var dict = new Dictionary<int, Func<Vector, int>>
	{
		{ dest.dx, v => v.dx },
		{ dest.dy, v => v.dy },
		{ dest.dz, v => v.dz },
		{ -dest.dx, v => -v.dx },
		{ -dest.dy, v => -v.dy },
		{ -dest.dz, v => -v.dz },
	};

	var xTranslation = dict[source.dx];
	var yTranslation = dict[source.dy];
	var zTranslation = dict[source.dz];
	return v => new Vector(xTranslation(v), yTranslation(v), zTranslation(v));
}

IEnumerable<Scanner> ParseScanners(string[] lines)
{
	int scannerId = 0;
	var points = new List<Point3D>();

	foreach (var line in lines)
	{
		if (line.StartsWith("---"))
		{
			scannerId = int.Parse(line.Replace("--- scanner ", "").Replace(" ---", ""));
			continue;
		}

		if (string.IsNullOrEmpty(line.Trim()))
		{
			yield return new Scanner(scannerId, points);
			points = new List<Point3D>();
			continue;
		}

		var point = line.Split(",");
		points.Add(new Point3D(int.Parse(point[0]), int.Parse(point[1]), int.Parse(point[2])));
	}

	yield return new Scanner(scannerId, points);
}