internal record Vector(int dx, int dy, int dz)
{
	public Vector(Point3D end, Point3D start)
		: this(end.x - start.x, end.y - start.y, end.z - start.z)
	{
	}

	public Vector(Point3D point)
		: this(point.x, point.y, point.z)
	{
	}

	public double Length = Math.Sqrt(dx * dx + dy * dy + dz * dz);

	public static Vector operator -(Vector vector) => new Vector(-vector.dx, -vector.dy, -vector.dz);
}
