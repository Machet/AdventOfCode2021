var input = File.ReadAllLines("input.txt");

var variables = new Dictionary<string, Operation>()
{
	{ "w", new InputOperation("w") },
	{ "x", new InputOperation("x") },
	{ "y", new InputOperation("y") },
	{ "z", new ConstOperation(0) },
};

int argumentCount = 0;

foreach (var i in input)
{
	var parts = i.Split(' ');
	var result = parts[1];
	var left = variables[parts[1]];
	var right = parts[0] != "inp"
		? int.TryParse(parts[2], out var constant) ? new ConstOperation(constant) : variables[parts[2]]
		: new ConstOperation(0); // not used

	Operation operation = parts[0] switch
	{
		"inp" => new InputOperation("arg" + argumentCount++),
		"add" => new AddOperation(left, right),
		"mul" => new MulOperation(left, right),
		"div" => new DivOperation(left, right),
		"mod" => new ModOperation(left, right),
		"eql" => new EqlOperation(left, right),
		_ => throw new Exception()
	};

	var x = operation.Reduce();
	variables[result] = x;
}

var calculations = variables["z"];
var isx = calculations.IsResultPossible(0).ToList();

Console.WriteLine(calculations);

record PotentialOutput(int value, Dictionary<string, int> variables)
{
	internal Dictionary<string, int> Mege(PotentialOutput rightOutput)
	{
		return variables.Concat(rightOutput.variables).Distinct().ToDictionary(x => x.Key, x => x.Value);
	}
}

abstract record Operation
{
	public abstract int GetInputCount();
	public abstract Operation Reduce();
	public abstract IEnumerable<PotentialOutput> IsResultPossible(int result);
	public abstract IEnumerable<PotentialOutput> GetPossibleOutputs();

	protected (Operation first, Operation second) GetCloser(Operation o1, Operation o2)
	{
		return o1.GetInputCount() < o2.GetInputCount() ? (o1, o2) : (o2, o1);
	}
}
