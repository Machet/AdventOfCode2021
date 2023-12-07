var open = new HashSet<char>() { '(', '[', '{', '<' };
var close = new HashSet<char>() { ')', ']', '}', '>' };

var inputs = File.ReadAllLines("input.txt").ToList();

var result1 = inputs
	.Select(i => FindIllegalCharacter(i))
	.Where(c => c != null)
	.Select(c => GetScore(c!.Value))
	.Sum();

Console.WriteLine("Result 1: " + result1);

var scores = inputs
	.Select(i => FindCompletion(i).ToList())
	.Select(c => GetCompletionScore(c))
	.Where(s => s != 0)
	.OrderBy(s => s)
	.ToList();

var result2 = scores[scores.Count / 2];

Console.WriteLine("Result 2: " + result2);

char? FindIllegalCharacter(string input)
{
	var openedSections = new Stack<char>();

	foreach (var c in input)
	{
		if (open.Contains(c))
		{
			openedSections.Push(c);
		}

		if (close.Contains(c))
		{
			var expected = GetExpected(openedSections.Pop());
			if (expected != c)
			{
				return c;
			}
		}
	}

	return null;
}

IEnumerable<char> FindCompletion(string input)
{
	var openedSections = new Stack<char>();

	foreach (var c in input)
	{
		if (open.Contains(c))
		{
			openedSections.Push(c);
		}

		if (close.Contains(c))
		{
			var expected = GetExpected(openedSections.Pop());
			if (expected != c)
			{
				yield break;
			}
		}
	}

	while (openedSections.Count > 0)
	{
		yield return GetExpected(openedSections.Pop());
	}
}

char GetExpected(char c)
{
	return c switch
	{
		'(' => ')',
		'[' => ']',
		'{' => '}',
		'<' => '>',
		_ => throw new Exception()
	};
}

int GetScore(char c)
{
	return c switch
	{
		')' => 3,
		']' => 57,
		'}' => 1197,
		'>' => 25137,
		_ => 0
	};
}

long GetCompletionScore(IEnumerable<char> completion)
{
	long result = 0;

	foreach (var c in completion)
	{
		var score = c switch
		{
			')' => 1,
			']' => 2,
			'}' => 3,
			'>' => 4,
			_ => 0
		};

		result = result * 5 + score;
	}

	return result;
}
