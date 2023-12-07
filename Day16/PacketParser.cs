internal class PacketParser
{
	private readonly BinaryReader _reader;

	public PacketParser(BinaryReader reader)
	{
		_reader = reader;
	}

	public Packet ParsePacket()
	{
		var version = _reader.ReadInt(3);
		var type = _reader.ReadInt(3);
		return type == 4
			? ParseLiteralValuePacket(version, type)
			: ParseOperatorPacket(version, type);
	}

	private Packet ParseLiteralValuePacket(int version, int type)
	{
		var value = new string(ParseFrames().ToArray());
		return new LiteralValuePacket(version, type, Convert.ToInt64(value, 2));
	}

	private Packet ParseOperatorPacket(int version, int type)
	{
		var lengthType = _reader.ReadByte();
		var packets = lengthType == '0'
			? ParseSubPackets1()
			: ParseSubPackets2();

		return new OperatorPacket(version, type, packets.ToList());
	}

	private IEnumerable<Packet> ParseSubPackets1()
	{
		var totalLength = _reader.ReadInt(15);
		var currentPos = _reader.Position;
		while (_reader.Position < currentPos + totalLength)
		{
			yield return ParsePacket();
		}
	}

	private IEnumerable<Packet> ParseSubPackets2()
	{
		var subPacketsCount = _reader.ReadInt(11);
		for (int i = 0; i < subPacketsCount; i++)
		{
			yield return ParsePacket();
		}
	}

	private IEnumerable<char> ParseFrames()
	{
		LiteralValueFrame frame = null;

		do
		{
			frame = _reader.ReadLiteralValueFrame();
			foreach (var b in frame.bytes)
			{
				yield return b;
			}
		}
		while (!frame.isLast);
	}
}
