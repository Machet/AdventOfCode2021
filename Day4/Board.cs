internal class Board
{
	public HashSet<int> MarkedNumbers { get; }
	public List<List<int>> Rows { get; }
	public List<List<int>> Cols { get; }

	public Board(string text)
	{
		MarkedNumbers = new HashSet<int>();

		Rows = text
			.Split(Environment.NewLine)
			.Select(x => x.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToList())
			.Where(x => x.Any())
			.ToList();

		Cols = Rows.SelectMany(r => r.Select((val, pos) => (val, pos)))
			.GroupBy(x => x.pos)
			.Select(x => x.Select(x => x.val).ToList())
			.ToList();
	}

	internal void Mark(int number)
	{
		if (Rows.Any(r => r.Contains(number)))
		{
			MarkedNumbers.Add(number);
		}
	}

	internal int GetScore()
	{
		return Rows.SelectMany(r => r).Where(n => !MarkedNumbers.Contains(n)).Sum();
	}

	internal bool HasBingo()
	{
		return Rows.Any(r => r.All(n => MarkedNumbers.Contains(n))) || Cols.Any(c => c.All(n => MarkedNumbers.Contains(n)));
	}
}