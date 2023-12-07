namespace Util
{
	public record ArrayCell<T>(int x, int y, T value)
	{
		public Point Point => new Point(x, y);
	}
}