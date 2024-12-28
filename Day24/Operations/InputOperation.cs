record InputOperation(string argName) : Operation
{
	public override int GetInputCount() => 1;
	public override Operation Reduce() => this;

	public override IEnumerable<PotentialOutput> GetPossibleOutputs()
	{
		yield return new PotentialOutput(1, new() { { argName, 1 } });
		yield return new PotentialOutput(2, new() { { argName, 2 } });
		yield return new PotentialOutput(3, new() { { argName, 3 } });
		yield return new PotentialOutput(4, new() { { argName, 4 } });
		yield return new PotentialOutput(5, new() { { argName, 5 } });
		yield return new PotentialOutput(6, new() { { argName, 6 } });
		yield return new PotentialOutput(7, new() { { argName, 7 } });
		yield return new PotentialOutput(8, new() { { argName, 8 } });
		yield return new PotentialOutput(9, new() { { argName, 9 } });
	}

	public override IEnumerable<PotentialOutput> IsResultPossible(int result)
	{
		if (result >= 1 && result <= 9)
		{
			yield return new PotentialOutput(result, new() { { argName, result } });
		}
	}
}
