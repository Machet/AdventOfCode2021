public record LiteralValueFrame(bool isLast, IEnumerable<char> bytes);

public abstract record Packet(int version, int type)
{
	public virtual long GetTotalVersion() => version;
	public abstract long GetValue();
}

public record LiteralValuePacket(int version, int type, long value) : Packet(version, type)
{
	public override long GetValue() => value;
}

public record OperatorPacket(int version, int type, List<Packet> packets) : Packet(version, type)
{
	public override long GetTotalVersion()
	{
		return version + packets.Select(p => p.GetTotalVersion()).Sum();
	}

	public override long GetValue()
	{
		return type switch
		{
			0 => packets.Select(p => p.GetValue()).Sum(),
			1 => packets.Aggregate((long)1, (a, p) => a * p.GetValue()),
			2 => packets.Select(p => p.GetValue()).Min(),
			3 => packets.Select(p => p.GetValue()).Max(),
			5 => packets[0].GetValue() > packets[1].GetValue() ? 1 : 0,
			6 => packets[0].GetValue() < packets[1].GetValue() ? 1 : 0,
			7 => packets[0].GetValue() == packets[1].GetValue() ? 1 : 0,
			_ => throw new ArgumentException()
		};
	}
}
