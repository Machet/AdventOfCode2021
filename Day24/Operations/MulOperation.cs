record MulOperation(Operation left, Operation right) : Operation
{
	public override int GetInputCount() => left.GetInputCount() + right.GetInputCount();

	public override Operation Reduce()
	{
		if (left is ConstOperation lConst && right is ConstOperation rConst)
		{
			return new ConstOperation(lConst.value * rConst.value);
		}

		if (left is ConstOperation lConst1 && lConst1.value == 0)
		{
			return new ConstOperation(0);
		}

		if (right is ConstOperation rConst1 && rConst1.value == 0)
		{
			return new ConstOperation(0);
		}

		return this;
	}

	public override IEnumerable<PotentialOutput> GetPossibleOutputs()
	{
		foreach (var leftOutput in left.GetPossibleOutputs())
		{
			foreach (var rightOutput in right.GetPossibleOutputs())
			{
				yield return new PotentialOutput(leftOutput.value * rightOutput.value, leftOutput.Mege(rightOutput));
			}
		}
	}

	public override IEnumerable<PotentialOutput> IsResultPossible(int result)
	{
		var (first, second) = GetCloser(left, right);

		foreach (var output in first.GetPossibleOutputs())
		{
			var desiredVal = output.value == 0 ? 0 : result / output.value;
			if (result % output.value != 0)
			{
				continue;
			}

			foreach (var match in second.IsResultPossible(desiredVal))
			{
				yield return new PotentialOutput(result, output.Mege(match));
			}
		}
	}
}
