


public record SnailfishNumberPair(ISnailFishNumber left, ISnailFishNumber right) : ISnailFishNumber
{
	public (bool exploded, ISnailFishNumber number, int left, int right) Explode(int depth)
	{
		if (left is SnailfishNumberRegular l && right is SnailfishNumberRegular r && depth > 4)
		{
			return (true, new SnailfishNumberRegular(0), l.value, r.value);
		}

		var leftResult = left.Explode(depth + 1);
		if (leftResult.exploded) return (true, new SnailfishNumberPair(leftResult.number, right.WithLeftIncreased(leftResult.right)), leftResult.left, 0);

		var rightResult = right.Explode(depth + 1);
		if (rightResult.exploded) return (true, new SnailfishNumberPair(left.WithRightIncresed(rightResult.left), rightResult.number), 0, rightResult.right);

		return (false, this, 0, 0);
	}

	public ISnailFishNumber Reduce()
	{
		(var hasChanged, var number, _, _) = Explode(1);

		while (hasChanged)
		{
			(hasChanged, number, _, _) = number.Explode(1);
			if (hasChanged) continue;

			(hasChanged, number) = number.Split();
		}

		return number;
	}

	public (bool, ISnailFishNumber) Split()
	{
		var result = left.Split();
		if (result.splitted) return (true, new SnailfishNumberPair(result.number, right));

		var result2 = right.Split();
		if (result2.splitted) return (true, new SnailfishNumberPair(left, result2.number));

		return (false, this);
	}

	public ISnailFishNumber Add(ISnailFishNumber value)
	{
		return new SnailfishNumberPair(this, value).Reduce();
	}

	public ISnailFishNumber WithLeftIncreased(int value)
	{
		return new SnailfishNumberPair(left.WithLeftIncreased(value), right);
	}

	public ISnailFishNumber WithRightIncresed(int value)
	{
		return new SnailfishNumberPair(left, right.WithRightIncresed(value));
	}

	public int GetMagnitude()
	{
		return 3 * left.GetMagnitude() + 2 * right.GetMagnitude();
	}

	public override string ToString()
	{
		return $"[{left},{right}]";
	}
}