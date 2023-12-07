namespace Util
{
	public static class Array
	{
		public static char[,] ToArray2D(this IEnumerable<string> vals)
		{
			var length = vals.First().Length;
			var result = new char[length, vals.Count()];
			var i = 0;

			foreach (var val in vals)
			{
				for (int pos = 0; pos < length; pos++)
				{
					result[pos, i] = val[pos];
				}

				i++;
			}

			return result;
		}

		public static R[,] Map<T, R>(this T[,] array, Func<T, R> map)
		{
			var result = new R[array.GetLength(0), array.GetLength(1)];

			for (int x = 0; x < array.GetLength(0); x++)
			{
				for (int y = 0; y < array.GetLength(1); y++)
				{
					result[x, y] = map(array[x, y]);
				}
			}

			return result;
		}

		public static IEnumerable<R> Select<T, R>(this T[,] array, Func<T, R> map)
		{
			for (int x = 0; x < array.GetLength(0); x++)
			{
				for (int y = 0; y < array.GetLength(1); y++)
				{
					yield return map(array[x, y]);
				}
			}
		}

		public static IEnumerable<R> Select<T, R>(this T[,] array, Func<int, int, T, R> map)
		{
			for (int x = 0; x < array.GetLength(0); x++)
			{
				for (int y = 0; y < array.GetLength(1); y++)
				{
					yield return map(x, y, array[x, y]);
				}
			}
		}

		public static IEnumerable<ArrayCell<T>> ToCells<T>(this T[,] array)
		{
			for (int y = 0; y < array.GetLength(1); y++)
			{
				for (int x = 0; x < array.GetLength(0); x++)
				{
					yield return new ArrayCell<T>(x, y, array[x, y]);
				}
			}
		}

		public static IEnumerable<T> GetCloseNeighbours<T>(this T[,] array, int x, int y)
		{
			if (x > 0)
			{
				yield return array[x - 1, y];
			}

			if (x < array.GetLength(0) - 1)
			{
				yield return array[x + 1, y];
			}

			if (y > 0)
			{
				yield return array[x, y - 1];
			}

			if (y < array.GetLength(1) - 1)
			{
				yield return array[x, y + 1];
			}
		}

		public static IEnumerable<T> GetAllNeighbours<T>(this T[,] array, int x, int y)
		{
			for (int i = Math.Max(0, x - 1); i < Math.Min(array.GetLength(0), x + 1); i++)
				for (int j = Math.Max(0, y - 1); j < Math.Min(array.GetLength(1), y + 1); j++)
				{
					if (i != x || j != y)
					{
						yield return array[i, j];
					}
				}
		}

		public static IEnumerable<ArrayCell<T>> GetNeighbourCells<T>(this T[,] array, int x, int y)
		{
			if (x > 0)
			{
				yield return new ArrayCell<T>(x - 1, y, array[x - 1, y]);
			}

			if (x < array.GetLength(0) - 1)
			{
				yield return new ArrayCell<T>(x + 1, y, array[x + 1, y]);
			}

			if (y > 0)
			{
				yield return new ArrayCell<T>(x, y - 1, array[x, y - 1]);
			}

			if (y < array.GetLength(1) - 1)
			{
				yield return new ArrayCell<T>(x, y + 1, array[x, y + 1]);
			}
		}

		public static IEnumerable<ArrayCell<T>> GetAllNeighbourCells<T>(this T[,] array, int x, int y)
		{
			for (int i = Math.Max(0, x - 1); i <= Math.Min(array.GetLength(0) - 1, x + 1); i++)
				for (int j = Math.Max(0, y - 1); j <= Math.Min(array.GetLength(1) - 1, y + 1); j++)
				{
					if (i != x || j != y)
					{
						yield return new ArrayCell<T>(i, j, array[i, j]);
					}
				}
		}

		public static void WriteToConsole<T>(this T[,] array)
		{
			for (int j = 0; j < array.GetLength(1); j++)
			{
				for (int i = 0; i < array.GetLength(0); i++)
				{
					Console.Write(array[i, j] + " ");
				}

				Console.WriteLine();
			}
		}
	}
}