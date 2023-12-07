var input = File.ReadAllText("input.txt")
	.Split(Environment.NewLine + Environment.NewLine)
	.ToList();

var numbers = input[0].Split(",").Select(x => int.Parse(x)).ToList();
var boards = input.Skip(1).Select(x => new Board(x)).ToList();

PlayBestBingo();
PlayWorstBingo();

void PlayBestBingo()
{
	foreach (var number in numbers)
	{
		foreach (var board in boards)
		{
			board.Mark(number);
			if (board.HasBingo())
			{
				Console.WriteLine("BEST BINGOOO " + board.GetScore() * number);
				return;
			}
		}
	}
}

void PlayWorstBingo()
{
	foreach (var number in numbers)
	{
		var boardsWithoutBingo = boards.Where(b => !b.HasBingo()).ToList();

		foreach (var board in boardsWithoutBingo)
		{
			board.Mark(number);

			if (board.HasBingo() && boardsWithoutBingo.Count() == 1)
			{
				Console.WriteLine("WORST BINGOOO " + board.GetScore() * number);
				return;
			}
		}
	}
}
