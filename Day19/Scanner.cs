internal record Point3D(int x, int y, int z)
{
	public static Point3D operator +(Point3D point, Vector vector)
		=> new Point3D(point.x + vector.dx, point.y + vector.dy, point.z + vector.dz);
}

internal record Vector(int dx, int dy, int dz)
{
	public Vector(Point3D end, Point3D start)
		: this(end.x - start.x, end.y - start.y, end.z - start.z)
	{
	}

	public double Length = Math.Sqrt(dx * dx + dy * dy + dz * dz);

	public static Vector operator -(Vector vector) => new Vector(-vector.dx, -vector.dy, -vector.dz);
}
internal record Scanner(int id, List<Point3D> beacons)
{
	public Dictionary<Point3D, HashSet<Vector>> Vectors = beacons
		.ToDictionary(b => b, b => beacons.Where(bb => bb != b).Select(bb => new Vector(bb, b)).ToHashSet());

	public void AddPoints(Point3D origin, ICollection<Vector> relativePoints)
	{
		foreach (var relation in relativePoints)
		{
			if (!Vectors[origin].Add(relation)) 
			{
				continue;
			}

			var dest = origin + relation;

			if (!Vectors.ContainsKey(dest))
			{
				Vectors[dest] = Vectors.Keys.Select(v => new Vector(v, dest)).ToHashSet();
			}

			foreach (var point in Vectors.Keys)
			{
				Vectors[point].Add(new Vector(dest, point));
			}
		}
	}
}
