using Util;

internal record Scanner(int id, List<Point3D> beacons)
{
	public Dictionary<Point3D, HashSet<Vector>> BeaconViews = beacons
		.ToDictionary(b => b, b => beacons.Where(bb => bb != b).Select(bb => new Vector(bb, b)).ToHashSet());
}
