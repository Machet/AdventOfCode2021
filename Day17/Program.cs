// target area: x=269..292, y=-68..-44

//var minX = 20;
//var maxX = 30;
//var maxY = -5;
//var minY = -10;

var minX = 269;
var maxX = 292;
var minY = -68;
var maxY = -44;


var result1 = 0;
for (int i = -minY - 1; i > 0; i--)
{
	result1 = result1 + i;
}

var validYs = Enumerable.Range(minY, -minY * 2)
	.SelectMany(v => IsInYRange(v, minY, maxY))
	.ToList();

var validXs = Enumerable.Range(0, maxX + 1)
	.SelectMany(v => IsInXRange(v, minX, maxX, validYs.Select(y => y.time).Max()))
	.ToList();

var result2 = validXs.Join(validYs, x => x.time, y => y.time, (x, y) => (x.v, y.v)).Distinct().ToList();

Console.WriteLine("1: " + result1);
Console.WriteLine("2: " + result2.Count());

IEnumerable<(int v, int time)> IsInXRange(int velocity, int min, int max, int timeToCount)
{
	var pos = 0;
	var time = 0;
	var v = velocity;
	for (int i = 0; i <= timeToCount; i++)
	{
		pos += v;
		v = v > 0 ? v - 1 : 0;
		time++;

		if (min <= pos && pos <= max)
		{
			yield return (velocity, time);
		}
	}
}

IEnumerable<(int v, int time)> IsInYRange(int velocity, int min, int max)
{
	var v = velocity;
	var pos = 0;
	var time = 0;
	while (pos >= min)
	{
		pos += v;
		v = v - 1;
		time++;

		if (min <= pos && pos <= max)
		{
			yield return (velocity, time);
		}
	}
}
