record DivOperation(Operation left, Operation right) : Operation
{
	public override int GetInputCount() => left.GetInputCount() + right.GetInputCount();

	public override Operation Reduce()
	{
		if (left is ConstOperation lConst && right is ConstOperation rConst)
		{
			return new ConstOperation(lConst.value / rConst.value);
		}

		if (right is ConstOperation rConst1 && rConst1.value == 1)
		{
			return left;
		}

		return this;
	}

	public override IEnumerable<PotentialOutput> GetPossibleOutputs()
	{
		foreach (var leftOutput in left.GetPossibleOutputs())
		{
			foreach (var rightOutput in right.GetPossibleOutputs().Where(o => o.value != 0))
			{
				yield return new PotentialOutput(leftOutput.value / rightOutput.value, leftOutput.Mege(rightOutput));
			}
		}
	}

	public override IEnumerable<PotentialOutput> IsResultPossible(int result)
	{
		return GetPossibleOutputs().Where(o => o.value == result);
	}
}
