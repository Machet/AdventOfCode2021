


public record SnailfishNumberRegular(int value) : ISnailFishNumber
{
	public (bool exploded, ISnailFishNumber number, int left, int right) Explode(int depth)
	{
		return (false, this, 0, 0);
	}

	public (bool splitted, ISnailFishNumber number) Split()
	{
		return value > 9
			? (true, new SnailfishNumberPair(new SnailfishNumberRegular(value / 2), new SnailfishNumberRegular((value + 1) / 2)))
			: (false, this);
	}

	public ISnailFishNumber Add(ISnailFishNumber value)
	{
		return new SnailfishNumberPair(this, value).Reduce();
	}

	public ISnailFishNumber WithLeftIncreased(int value)
	{
		return new SnailfishNumberRegular(this.value + value);
	}

	public ISnailFishNumber WithRightIncresed(int value)
	{
		return new SnailfishNumberRegular(this.value + value);
	}

	public int GetMagnitude()
	{
		return value;
	}

	public ISnailFishNumber Reduce()
	{
		return Split().number;
	}

	public override string ToString()
	{
		return value.ToString();
	}
}
