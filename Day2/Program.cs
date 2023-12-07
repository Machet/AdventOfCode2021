var commands = File.ReadAllLines("input.txt")
	.Select(x => x.Split(" "))
	.Select(x => new Command(x[0], long.Parse(x[1])))
	.ToList();

long depth = 0;
long pos = 0;
long aim = 0;

foreach (var command in commands)
{
	if (command.action == "forward")
	{
		pos += command.value;
		depth += aim * command.value;
	}

	if (command.action == "down")
	{
		aim += command.value;
	}

	if (command.action == "up")
	{
		aim -= command.value;
	}
}


Console.WriteLine($"Depth {depth} Pos {pos} Result {pos * depth}");

record Command(string action, long value);