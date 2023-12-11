var numbers = File.ReadAllLines("input.txt")
	.Select(x => ParseSnailfishNumber(x.GetEnumerator()))
	.ToList();

var result1 = numbers[0];
for (int i = 1; i < numbers.Count; i++)
{
	result1 = result1.Add(numbers[i]);
}

var result2 = numbers
	.SelectMany(n => numbers.Where(nn => nn != n)
	.Select(nn => n.Add(nn).GetMagnitude()))
	.Max();

Console.WriteLine("1: " + result1.GetMagnitude());
Console.WriteLine("2: " + result2);

ISnailFishNumber ParseSnailfishNumber(CharEnumerator charEnumerator)
{
	charEnumerator.MoveNext();

	if (int.TryParse(charEnumerator.Current.ToString(), out var snailfishNumber))
	{
		return new SnailfishNumberRegular(snailfishNumber);
	}

	if (charEnumerator.Current != '[') throw new Exception();
	var left = ParseSnailfishNumber(charEnumerator);

	charEnumerator.MoveNext();
	if (charEnumerator.Current != ',') throw new Exception();

	var right = ParseSnailfishNumber(charEnumerator);

	charEnumerator.MoveNext();
	if (charEnumerator.Current != ']') throw new Exception();

	return new SnailfishNumberPair(left, right);
}
