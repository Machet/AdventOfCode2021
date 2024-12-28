record AddOperation(Operation left, Operation right) : Operation
{
	public override int GetInputCount() => left.GetInputCount() + right.GetInputCount();

	public override Operation Reduce()
	{
		if (left is ConstOperation lConst && right is ConstOperation rConst)
		{
			return new ConstOperation(lConst.value + rConst.value);
		}

		return this;
	}

	public override IEnumerable<PotentialOutput> GetPossibleOutputs()
	{
		foreach (var leftOutput in left.GetPossibleOutputs())
		{
			foreach (var rightOutput in right.GetPossibleOutputs())
			{
				yield return new PotentialOutput(leftOutput.value + rightOutput.value, leftOutput.Mege(rightOutput));
			}
		}
	}

	public override IEnumerable<PotentialOutput> IsResultPossible(int result)
	{
		var (first, second) = GetCloser(left, right);

		foreach (var output in first.GetPossibleOutputs())
		{
			foreach (var match in second.IsResultPossible(result - output.value))
			{
				yield return new PotentialOutput(result, output.Mege(match));
			}
		}
	}
}
