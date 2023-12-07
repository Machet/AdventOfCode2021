var input = File.ReadAllText("input.txt");
var binaryPacket = HexStringToBinary(input).ToList();
var parser = new PacketParser(new BinaryReader(binaryPacket));
var packet = parser.ParsePacket();

Console.WriteLine("1: " + packet.GetTotalVersion());
Console.WriteLine("2: " + packet.GetValue());

static IEnumerable<char> HexStringToBinary(string hexString)
{
	foreach (var c in hexString)
	{
		foreach (var z in HexToBinary(c))
		{
			yield return z;
		}
	}
}

static string HexToBinary(char hexChar)
{
	return hexChar switch
	{
		'0' => "0000",
		'1' => "0001",
		'2' => "0010",
		'3' => "0011",
		'4' => "0100",
		'5' => "0101",
		'6' => "0110",
		'7' => "0111",
		'8' => "1000",
		'9' => "1001",
		'A' => "1010",
		'B' => "1011",
		'C' => "1100",
		'D' => "1101",
		'E' => "1110",
		'F' => "1111",
		_ => throw new ArgumentException()
	};
}
