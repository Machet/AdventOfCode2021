using Util;

internal class OceanView
{
	public Dictionary<int, Point3D> Scanners { get; }
	public Dictionary<Point3D, HashSet<Vector>> BeaconViews { get; }

	public OceanView(Scanner scanner)
	{
		Scanners = new Dictionary<int, Point3D>() { { scanner.id, new Point3D(0, 0, 0) } };
		BeaconViews = scanner.beacons.ToDictionary(b => b, b => scanner.beacons.Where(bb => bb != b).Select(bb => new Vector(bb, b)).ToHashSet());
	}

	internal void AddScannerRelativeTo(Point3D point, int scannerId, Vector relation)
	{
		var scannerPosition = point + relation;
		Scanners.Add(scannerId, scannerPosition);
	}

	public void AddBeaconsRelativeTo(Point3D origin, ICollection<Vector> relativePoints)
	{
		foreach (var relation in relativePoints)
		{
			if (!BeaconViews[origin].Add(relation))
			{
				continue;
			}

			var dest = origin + relation;

			if (!BeaconViews.ContainsKey(dest))
			{
				BeaconViews[dest] = BeaconViews.Keys.Select(v => new Vector(v, dest)).ToHashSet();
			}

			foreach (var point in BeaconViews.Keys)
			{
				BeaconViews[point].Add(new Vector(dest, point));
			}
		}
	}

	internal int GetBeaconCount()
	{
		return BeaconViews.Count;
	}

	internal int GetMaxDistBetweenScanners()
	{
		return Scanners.Values.GenerateAllCombinations()
			.Select(c => Math.Abs(c.first.x - c.second.x) + Math.Abs(c.first.y - c.second.y) + Math.Abs(c.first.z - c.second.z))
			.Max();
	}
}
