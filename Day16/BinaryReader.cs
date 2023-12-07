public class BinaryReader
{
	private IEnumerator<char> _enumerator;
	private int _index = -1;

	public BinaryReader(IEnumerable<char> binaryString)
	{
		_enumerator = binaryString.GetEnumerator();
	}

	public int Position => _index;
	public LiteralValueFrame ReadLiteralValueFrame() => new LiteralValueFrame(ReadByte() == '0', Take(4).ToList());
	public int ReadInt(int byteCount) => Convert.ToInt32(new string(Take(byteCount).ToArray()), 2);
	public char ReadByte() => Take(1).First();

	public IEnumerable<char> Take(int count)
	{
		for (int i = 0; i < count; i++)
		{
			_enumerator.MoveNext();
			_index++;
			yield return _enumerator.Current;
		}
	}
}
