var input = File.ReadAllLines("input.txt");
var algorithm = input[0];

var points = input.Skip(2).SelectMany((line, i) => ParsePoints(line, i)).ToHashSet();
var inputImg = new Image(0, input.Length - 2, 0, input[2].Length, points);

var resImg = inputImg;
var pointCounts = new Dictionary<int, int>();

for (int i = 1; i <= 50; i++)
{
	var extend = i % 2 == 1 ? 6 : -3;
	resImg = EnchanceImage(resImg, algorithm, extend);
	pointCounts[i] = resImg.points.Count;
}

Console.WriteLine("1: " + pointCounts[2]);
Console.WriteLine("2: " + pointCounts[50]);

static Image EnchanceImage(Image img, string algorithm, int extend)
{
	var destImg = new Image(img.startX - extend, img.endX + extend, img.startY - extend, img.endY + extend, new HashSet<Point>());

	for (int i = destImg.startX; i <= destImg.endX; i++)
	{
		for (int j = destImg.startY; j <= destImg.endY; j++)
		{
			var hashIndex = GetHashFor(i, j, img.points);
			if (algorithm[hashIndex] == '#')
			{
				destImg.points.Add(new Point(i, j));
			}
		}
	}

	return destImg;
}

static int GetHashFor(int pointX, int pointY, HashSet<Point> points)
{
	return GetTemplatePoints(pointX, pointY)
		.Select(x => points.Contains(x) ? 1 : 0)
		.Aggregate((acc, item) => acc * 2 + item);
}

static IEnumerable<Point> GetTemplatePoints(int pointX, int pointY)
{
	yield return new Point(pointX - 1, pointY - 1);
	yield return new Point(pointX - 1, pointY);
	yield return new Point(pointX - 1, pointY + 1);
	yield return new Point(pointX, pointY - 1);
	yield return new Point(pointX, pointY);
	yield return new Point(pointX, pointY + 1);
	yield return new Point(pointX + 1, pointY - 1);
	yield return new Point(pointX + 1, pointY);
	yield return new Point(pointX + 1, pointY + 1);
}

static IEnumerable<Point> ParsePoints(string line, int number)
{
	return line.Select((c, i) => (c, i))
		.Where(r => r.c == '#')
		.Select(r => new Point(number, r.i));
}

record Point(int x, int y);
record Image(int startX, int endX, int startY, int endY, HashSet<Point> points)
{
	public void Print()
	{
		for (int i = startX; i < endX; i++)
		{
			for (int j = startY; j < endY; j++)
			{
				Console.Write(points.Contains(new Point(i, j)) ? '#' : '.');
			}

			Console.WriteLine();
		}
	}
}