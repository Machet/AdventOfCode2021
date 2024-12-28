record ConstOperation(int value) : Operation
{
	public override int GetInputCount() => 0;
	public override Operation Reduce() => this;

	public override IEnumerable<PotentialOutput> GetPossibleOutputs()
	{
		yield return new PotentialOutput(value, new());
	}

	public override IEnumerable<PotentialOutput> IsResultPossible(int result)
	{
		if (value == result)
		{
			yield return new PotentialOutput(value, new());
		}
	}
}
