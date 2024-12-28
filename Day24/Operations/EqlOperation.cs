record EqlOperation(Operation left, Operation right) : Operation
{
	public override int GetInputCount() => left.GetInputCount() + right.GetInputCount();

	public override Operation Reduce()
	{
		return this;
	}

	public override IEnumerable<PotentialOutput> GetPossibleOutputs()
	{
		foreach (var leftOutput in left.GetPossibleOutputs())
		{
			foreach (var rightOutput in right.GetPossibleOutputs())
			{
				yield return new PotentialOutput(leftOutput.value == rightOutput.value ? 1 : 0, leftOutput.Mege(rightOutput));
			}
		}
	}

	public override IEnumerable<PotentialOutput> IsResultPossible(int result)
	{
		if (result == 0)
		{
			foreach (var x in GetPossibleOutputs().Where(o => o.value == result))
			{
				yield return x;
			}
		}

		if (result == 1)
		{
			var (first, second) = GetCloser(left, right);
			foreach (var output in first.GetPossibleOutputs())
			{
				foreach (var matching in second.IsResultPossible(output.value))
				{
					yield return new PotentialOutput(1, output.Mege(matching));
				}
			}
		}
	}
}
