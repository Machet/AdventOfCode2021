var diagnostics = File.ReadAllLines("input.txt")
	.ToList();

var statistics = GetStatistics(diagnostics)
	.ToList();

long gamma = 0;
long epsilon = 0;

foreach (var diag in statistics.OrderBy(x => x.pos))
{
	gamma = gamma * 2;
	epsilon = epsilon * 2;

	if (diag.mostCommon == '1')
	{
		gamma += 1;
	}
	else
	{
		epsilon += 1;
	}
}

Console.WriteLine($"Gamma {gamma} Epsilon {epsilon} Result {gamma * epsilon}");

int loop = 0;
var oxygenGeneratorSearch = diagnostics.AsEnumerable();

while (oxygenGeneratorSearch.Count() > 1)
{
	var stats = GetStatistics(oxygenGeneratorSearch).Where(x => x.pos == loop).First();

	oxygenGeneratorSearch = stats.areEqual
		? oxygenGeneratorSearch.Where(x => x[stats.pos] == '1')
		: oxygenGeneratorSearch.Where(x => x[stats.pos] == stats.mostCommon);
	
	loop++;
}

loop = 0;
var co2ScrubberSearch = diagnostics.AsEnumerable();
while (co2ScrubberSearch.Count() > 1)
{
	var stats = GetStatistics(co2ScrubberSearch).Where(x => x.pos == loop).First();

	co2ScrubberSearch = stats.areEqual
		? co2ScrubberSearch.Where(x => x[stats.pos] == '0')
		: co2ScrubberSearch.Where(x => x[stats.pos] == stats.lessCommon);

	loop++;
}

var oxygen = ToDecimal(oxygenGeneratorSearch.First());
var co2 = ToDecimal(co2ScrubberSearch.First());

Console.WriteLine($"Oxygen {oxygen} C02 {co2} Result {oxygen * co2}");


IEnumerable<BitStatistic> GetStatistics(IEnumerable<string> diagnostics)
{
	return diagnostics
		.SelectMany(x => x.Select((y, i) => new Bit(i, y)))
		.GroupBy(x => x.pos)
		.Select(x => GeBitStatistics(x.Key, x));
}

BitStatistic GeBitStatistics(int pos, IEnumerable<Bit> bits)
{
	var stats = bits.Select(b => b.val)
		.GroupBy(x => x)
		.Select(x => new { bit = x.Key, count = x.Count() })
		.OrderByDescending(x => x.count)
		.ToList();

	var areEqual = stats.Count > 1 && stats[0].count == stats[1].count;
	return new BitStatistic(pos, stats[0].bit, areEqual);
}

long ToDecimal(string val)
{
	long result = 0;
	foreach (var bit in val)
	{
		result = result * 2;
		if (bit == '1')
		{
			result += 1;
		}
	}

	return result;
}

record Bit(int pos, char val);
record BitStatistic(int pos, char mostCommon, bool areEqual)
{
	public char lessCommon = mostCommon == '1' ? '0' : '1';
};
