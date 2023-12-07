var measurements = File.ReadAllLines("input.txt")
	.Select(x => int.Parse(x))
	.ToList();

var groups = measurements.Zip(measurements.Skip(1), measurements.Skip(2))
	.Select(x => x.First + x.Second + x.Third)
	.ToList();

int result = 0;
for (int i = 1; i < groups.Count; i++)
{
	result += groups[i] > groups[i - 1] ? 1 : 0;
}

Console.WriteLine(result);
