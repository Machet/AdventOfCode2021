var inputs = File.ReadAllText("input.txt")
	.Split(",")
	.Select(x => int.Parse(x))
	.ToList();

var valuestToTest = Enumerable.Range(inputs.Min(), inputs.Max() - inputs.Min());

var min = valuestToTest.Select(x => GetValue(inputs, x)).Min();
Console.WriteLine(min);


var min2 = valuestToTest.Select(x => GetValue2(inputs, x)).Min();
Console.WriteLine(min2);


int GetValue(List<int> inputs, int val)
{
	return inputs.Select(i => Math.Abs(i - val)).Sum();
}

int GetValue2(List<int> inputs, int val)
{
	return inputs.Select(i => Math.Abs(i - val) == 0 ? 0 : (Math.Abs(i - val) + 1)  * Math.Abs(i - val) / 2).Sum();
}