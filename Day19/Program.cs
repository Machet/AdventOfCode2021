var scanners = ParseScanners(File.ReadAllLines("input.txt")).ToList();

var ocean = new OceanView(scanners[0]);
var scannersToProcess = new Queue<Scanner>(scanners.Skip(1));

while (scannersToProcess.Count > 0)
{
	var scanner = scannersToProcess.Dequeue();
	if (!ExpandWorld(ocean, scanner))
	{
		scannersToProcess.Enqueue(scanner);
	}
}

Console.WriteLine("1: " + ocean.GetBeaconCount());
Console.WriteLine("2: " + ocean.GetMaxDistBetweenScanners());

static bool ExpandWorld(OceanView ocean, Scanner scanner)
{
	foreach (var oceanBeaconView in ocean.BeaconViews)
	{
		var oceanBeaconLengthRelation = oceanBeaconView.Value.Select(x => x.Length);

		foreach (var scannerBeaconView in scanner.BeaconViews)
		{
			var commonRelations = scannerBeaconView.Value.IntersectBy(oceanBeaconLengthRelation, x => x.Length).ToList();

			if (commonRelations.Count > 9)
			{
				var dest = oceanBeaconView.Value.First(v => v.Length == commonRelations[0].Length);
				var source = commonRelations[0];
				var translation = GetTranslationFunction(dest, source);
				ocean.AddBeaconsRelativeTo(oceanBeaconView.Key, scannerBeaconView.Value.Select(translation).ToList());
				ocean.AddScannerRelativeTo(oceanBeaconView.Key, scanner.id, translation(-new Vector(scannerBeaconView.Key)));
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