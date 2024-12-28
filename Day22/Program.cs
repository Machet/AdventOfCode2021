using System.Text.RegularExpressions;

var commands = File.ReadAllLines("input.txt")
	.Select(Parse)
	.ToList();

var range1 = new Range(-50, 50);
var result1 = RunInitializationProcedure(commands, new Cube(range1, range1, range1));
Console.WriteLine("1: " + result1);

var range2 = new Range(int.MinValue, int.MaxValue);
var result2 = RunInitializationProcedure(commands, new Cube(range2, range2, range2));
Console.WriteLine("2: " + result2);


static long RunInitializationProcedure(IEnumerable<Command> commands, Cube affectedArea)
{
	var onCubes = new List<Cube>();
	foreach (var command in commands)
	{
		if (!command.Cube.Intersect(affectedArea))
		{
			continue;
		}

		var commandCube = command.Cube.Clamp(affectedArea);
		if (command.on)
		{
			var remainingCubesToAdd = new List<Cube> { commandCube };
			foreach (var onCube in onCubes)
			{
				remainingCubesToAdd = remainingCubesToAdd.Select(c => c.Except(onCube)).SelectMany(x => x).ToList();
			}

			onCubes.AddRange(remainingCubesToAdd);
		}
		else
		{
			onCubes = onCubes.Select(c => c.Except(commandCube)).SelectMany(x => x).ToList();
		}
	}

	return onCubes.Select(c => c.Volume).Sum();
}

static Command Parse(string line)
{
	var match = new Regex(@"(?<status>on|off) x=(?<xStart>-?\d+)..(?<xEnd>-?\d+),y=(?<yStart>-?\d+)..(?<yEnd>-?\d+),z=(?<zStart>-?\d+)..(?<zEnd>-?\d+)").Match(line);
	var on = match.Groups["status"].Value == "on";
	var xRange = new Range(int.Parse(match.Groups["xStart"].Value), int.Parse(match.Groups["xEnd"].Value));
	var yRange = new Range(int.Parse(match.Groups["yStart"].Value), int.Parse(match.Groups["yEnd"].Value));
	var zRange = new Range(int.Parse(match.Groups["zStart"].Value), int.Parse(match.Groups["zEnd"].Value));
	var cube = new Cube(xRange, yRange, zRange);
	return new Command(cube, on);
}

record Range(int Start, int End)
{
	public int Length => End - Start + 1;
	public bool Intersect(Range other) => Start <= other.End && End >= other.Start;
	public bool IsWithin(Range another) => Start >= another.Start && End <= another.End;
	public bool Contains(Range another) => another.IsWithin(this);
	public Range Clamp(Range another) => Intersect(another)
		? new Range(Math.Max(Start, another.Start), Math.Min(End, another.End))
		: throw new ArgumentException();

	public IEnumerable<Range> SplitByIntersection(Range other)
	{
		if (!Intersect(other)) throw new ArgumentException();

		var breakingPoints = new List<int>();

		if (other.Start != Start)
		{
			breakingPoints.Add(Start);
			breakingPoints.Add(other.Start);
			breakingPoints.Add(other.Start < Start ? Start - 1 : other.Start - 1);
		}
		else
		{
			breakingPoints.Add(Start);
		}

		if (other.End != End)
		{
			breakingPoints.Add(other.End);
			breakingPoints.Add(End);
			breakingPoints.Add(End < other.End ? End + 1 : other.End + 1);
		}
		else
		{
			breakingPoints.Add(End);
		}

		breakingPoints.Sort();

		for (int i = 1; i < breakingPoints.Count; i += 2)
		{
			yield return new Range(breakingPoints[i - 1], breakingPoints[i]);
		}
	}
}

record Cube(Range X, Range Y, Range Z)
{
	public long Volume = (long)X.Length * Y.Length * Z.Length;
	public bool Intersect(Cube other) => X.Intersect(other.X) && Y.Intersect(other.Y) && Z.Intersect(other.Z);
	public Cube Clamp(Cube other) => new Cube(X.Clamp(other.X), Y.Clamp(other.Y), Z.Clamp(other.Z));

	public IEnumerable<Cube> Except(Cube other)
	{
		if (!Intersect(other))
		{
			yield return this;
			yield break;
		}

		var xRanges = X.SplitByIntersection(other.X).ToArray();
		var yRanges = Y.SplitByIntersection(other.Y).ToArray();
		var zRanges = Z.SplitByIntersection(other.Z).ToArray();

		foreach (var x in xRanges)
		{
			foreach (var y in yRanges)
			{
				foreach (var z in zRanges)
				{
					var cube = new Cube(x, y, z);
					if (!cube.Intersect(other) && cube.Intersect(this))
					{
						yield return cube;
					}
				}
			}
		}
	}
}

record Command(Cube Cube, bool on);