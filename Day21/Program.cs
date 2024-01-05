var player1Position = 1;
var player2Position = 6;
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

Console.WriteLine("1: " + Math.Min(player1Score, player2Score) * turns * 3);

int GetNewPosition(int rolled, int position)
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