var player1Position = 1;
var player2Position = 6;

var result1 = PlayPracticeGame(player1Position, player2Position);
var result2 = PlayQuantumGame(player1Position, player2Position);

Console.WriteLine("1: " + result1);
Console.WriteLine("2: " + result2);

static long PlayQuantumGame(int player1Position, int player2Position)
{
	var possibleScores1 = GetPossibleScores(player1Position);
	var possibleScores2 = GetPossibleScores(player2Position);
	var p1Wins = 0L;
	var p2Wins = 0L;

	foreach (var p1WinsOnRound in possibleScores1.Where(s => s.Key.mayWin))
	{
		var p2LosesRoundBefore = possibleScores2.Where(p => p.Key.turn == p1WinsOnRound.Key.turn - 1).SingleOrDefault(p => !p.Key.mayWin);
		p1Wins += (long)p1WinsOnRound.Value * p2LosesRoundBefore.Value;
	}

	foreach (var p2WinsOnRound in possibleScores2.Where(s => s.Key.mayWin))
	{
		var p1LosesOnRound = possibleScores1.Where(p => p.Key.turn == p2WinsOnRound.Key.turn).SingleOrDefault(p => !p.Key.mayWin);
		p2Wins += (long)p2WinsOnRound.Value * p1LosesOnRound.Value;
	}

	return Math.Max(p1Wins, p2Wins);
}

static int PlayPracticeGame(int player1Position, int player2Position)
{
	var player1Score = 0;
	var player2Score = 0;
	var dice = GetDice().GetEnumerator();
	var player1Turn = true;
	var turns = 0;

	while (player1Score < 1000 && player2Score < 1000)
	{
		var rolled = GetRolls(3, dice);
		if (player1Turn)
		{
			player1Position = GetNewPosition(rolled, player1Position);
			player1Score += player1Position;
		}
		else
		{
			player2Position = GetNewPosition(rolled, player2Position);
			player2Score += player2Position;
		}

		player1Turn = !player1Turn;
		turns++;
	}

	return Math.Min(player1Score, player2Score) * turns * 3;
}

static Dictionary<TurnScore, int> GetPossibleScores(int initialPosition)
{
	var scores = new Dictionary<TurnScore, int>();
	var movements = GetPossibleMovements().GroupBy(m => m).Select(g => (steps: g.Key, stateCount: g.Count())).ToList();
	var toProcess = new Queue<TurnPosition>();
	toProcess.Enqueue(new TurnPosition(0, 0, initialPosition, 1));

	while (toProcess.Count > 0)
	{
		var state = toProcess.Dequeue();

		foreach(var movement in movements)
		{
			var newPos = GetNewPosition(movement.steps, state.position);
			var newScore = state.score + newPos;
			var mayWin = newScore >= 21;
			var score = new TurnScore(state.turn, mayWin);
			AddScore(scores, score, state.stateCount * movement.stateCount);

			if (!mayWin)
			{
				toProcess.Enqueue(new TurnPosition(state.turn + 1, newScore, newPos, state.stateCount * movement.stateCount));
			}
		}
	}

	return scores;
}

static void AddScore(Dictionary<TurnScore, int> scores, TurnScore score, int count)
{
	if (!scores.ContainsKey(score))
	{
		scores[score] = 0;
	}

	scores[score] = scores[score] + count;
}

static int GetNewPosition(int rolled, int position)
{
	var pos = position - 1 + rolled;
	return (pos % 10) + 1;
}

static int GetRolls(int count, IEnumerator<int> dice)
{
	var acc = 0;
	for (int i = 0; i < count; i++)
	{
		dice.MoveNext();
		acc += dice.Current;
	}

	return acc;
}

static IEnumerable<int> GetDice()
{
	var i = 1;
	while (true)
	{
		yield return i;
		i++;
		if (i > 100)
		{
			i = 1;
		}
	}
}

static IEnumerable<int> GetPossibleMovements(int throwCount = 3)
{
	var toProcess = new Queue<(int curr, int count)>();
	toProcess.Enqueue((0, throwCount));
	while (toProcess.Count > 0)
	{
		var item = toProcess.Dequeue();
		if (item.count == 0)
		{
			yield return item.curr;
			continue;
		}

		for (int i = 1; i <= 3; i++)
		{
			toProcess.Enqueue((item.curr + i, item.count - 1));
		}
	}
}

record TurnScore(int turn, bool mayWin);
record TurnPosition(int turn, int score, int position, int stateCount);