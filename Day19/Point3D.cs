internal record Point3D(int x, int y, int z)
{
	public static Point3D operator +(Point3D point, Vector vector)
		=> new Point3D(point.x + vector.dx, point.y + vector.dy, point.z + vector.dz);
}
