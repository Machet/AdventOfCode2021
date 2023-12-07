using System.Numerics;

var fishes = File.ReadAllText("input.txt")
	.Split(',')
	.Select(x => long.Parse(x))
	.GroupBy(x => x)
	.ToDictionary(x => x.Key, x => (BigInteger)x.Count());

for (int i = 0; i < 256; i++)
{
	var newFishes = fishes.GetValueOrDefault(0);
	fishes[0] = fishes.GetValueOrDefault(1);
	fishes[1] = fishes.GetValueOrDefault(2);
	fishes[2] = fishes.GetValueOrDefault(3);
	fishes[3] = fishes.GetValueOrDefault(4);
	fishes[4] = fishes.GetValueOrDefault(5);
	fishes[5] = fishes.GetValueOrDefault(6);
	fishes[6] = fishes.GetValueOrDefault(7) + newFishes;
	fishes[7] = fishes.GetValueOrDefault(8);
	fishes[8] = newFishes;
}

Console.WriteLine(fishes.Select(x => x.Value).Aggregate(BigInteger.Add));
