using System.Collections.Immutable;
using System.Text;

var input = File.ReadAllLines("input.txt")
	.Skip(2)
	.Take(2)
	.ToList();

var rooms = ParseRooms(input[0], input[1]).ToImmutableList();
var hallway = Enumerable.Repeat(default(char?), 11).ToImmutableList();

var states = new Dictionary<string, long>();
var queue = new Queue<State>();
queue.Enqueue(new State(hallway, rooms, 0, ImmutableList<State>.Empty));

var minEnergy = long.MaxValue;
var minState = queue.Peek();
while (queue.Count > 0)
{
	var state = queue.Dequeue();
	if (states.TryGetValue(state.GetStateString(), out var energy) && state.energy > energy)
	{
		continue;
	}

	if (state.IsComplete)
	{
		minEnergy = Math.Min(minEnergy, state.energy);
		minState = state;
	}
	else
	{
		foreach (var newState in state.GetNextStates())
		{
			var hashKey = newState.GetStateString();
			if (!states.TryGetValue(hashKey, out energy) || newState.energy < energy)
			{
				states[hashKey] = newState.energy;
				queue.Enqueue(newState);
			}
		}
	}
}

Console.WriteLine("1: " + minEnergy);

foreach (var state in minState.states)
{
	Console.Write("#");
	foreach (var i in state.hallway)
	{
		Console.Write(i ?? '.');
	}
	Console.WriteLine('#');
	Console.Write("  #");
	foreach (var i in state.rooms)
	{
		Console.Write(i.pos1 ?? '.');
		Console.Write('#');
	}

	Console.WriteLine();
	Console.Write("  #");
	foreach (var i in state.rooms)
	{
		Console.Write(i.pos2 ?? '.');
		Console.Write('#');
	}

	Console.WriteLine(state.energy);
	Console.WriteLine();
}

IEnumerable<Room> ParseRooms(string firstPos, string secondPos)
{
	var desired = new[] { 'A', 'B', 'C', 'D' }.GetEnumerator();

	for (int i = 0; i < firstPos.Length; i++)
	{
		if (firstPos[i] != '.' && firstPos[i] != '#')
		{
			desired.MoveNext();
			yield return new Room(i - 1, (char)desired.Current, firstPos[i], secondPos[i]);
		}
	}
}

record Room(int hallwayIndex, char desired, char? pos1, char? pos2)
{
	public bool IsComplete => pos1 == desired && pos2 == desired;
	public bool IsEmpty => pos1 == null && pos2 == null;
	public bool MayEnter => IsEmpty || (pos1 == null && pos2 == desired);
	public char? AmiphodToOut => pos1 ?? pos2;
	public int EscapeCost => pos1 != null ? 1 : 2;
	public int EnterCost => pos1 == null ? pos2 == null ? 2 : 1 : 0;

	internal Room WithAmiphodOut()
	{
		return pos1 != null
			? new Room(hallwayIndex, desired, null, pos2)
			: pos2 != null ? new Room(hallwayIndex, desired, null, null) : throw new Exception();
	}

	internal Room WithAmiphodIn()
	{
		return pos2 == null && pos1 == null
			? new Room(hallwayIndex, desired, null, desired)
			: pos1 == null ? new Room(hallwayIndex, desired, desired, pos2) : throw new Exception();
	}
}

record State(ImmutableList<char?> hallway, ImmutableList<Room> rooms, long energy, ImmutableList<State> states)
{
	public bool IsComplete = rooms.All(r => r.IsComplete);
	public HashSet<int> RoomIndexes = rooms.Select(r => r.hallwayIndex).ToHashSet();

	internal IEnumerable<State> GetNextStates()
	{
		foreach (var r in rooms.Where(r => !r.IsComplete && !r.IsEmpty))
		{
			var desiredRoom = rooms.First(rr => rr.desired == r.AmiphodToOut);
			foreach (var hIndex in GetAccessibleHallways(r).ToList())
			{
				var moveCost = GetCostRoomToHallway(r, hIndex) * GetMoveCost(r.AmiphodToOut);
				var newHallway = hallway.SetItem(hIndex, r.AmiphodToOut);
				var newRooms = rooms.Replace(r, r.WithAmiphodOut());
				yield return new State(newHallway, newRooms, energy + moveCost, states.Add(this));

				if (desiredRoom.hallwayIndex == hIndex && desiredRoom.MayEnter)
				{
					var newRooms2 = rooms
						.Replace(r, r.WithAmiphodOut())
						.Replace(desiredRoom, desiredRoom.WithAmiphodIn());

					var moveCost2 = GetCostRoomToRoom(r, desiredRoom) * GetMoveCost(r.AmiphodToOut);

					yield return new State(hallway, newRooms2, energy + moveCost2, states.Add(this));
				}
			}
		}

		foreach (var h in hallway.Select((h, i) => (amiphod: h, index: i)).Where((hi => hi.amiphod.HasValue)))
		{
			var desiredRoom = rooms.First(rr => rr.desired == h.amiphod);
			if (!desiredRoom.MayEnter) continue;

			var obstacle = false;
			for (int i = desiredRoom.hallwayIndex; i != h.index; i += desiredRoom.hallwayIndex < h.index ? 1 : -1)
			{
				if (hallway[i] != null)
				{
					obstacle = true;
				}
			}
			if (obstacle) continue;
			var newRooms = rooms.Replace(desiredRoom, desiredRoom.WithAmiphodIn());
			var newHallway = hallway.SetItem(h.index, null);
			var moveCost = GetCostHallwayToRoom(h.index, desiredRoom) * GetMoveCost(h.amiphod);

			yield return new State(newHallway, newRooms, energy + moveCost, states.Add(this));
		}
	}

	private long GetCostHallwayToRoom(int hIndex, Room room)
	{
		return room.EnterCost + Math.Abs(hIndex - room.hallwayIndex);
	}

	private long GetCostRoomToRoom(Room room1, Room room2)
	{
		return room1.EscapeCost + Math.Abs(room1.hallwayIndex - room2.hallwayIndex) + room2.EnterCost;
	}

	private long GetCostRoomToHallway(Room room, int hIndex)
	{
		return room.EscapeCost + Math.Abs(hIndex - room.hallwayIndex);
	}

	private long GetMoveCost(char? amiphod)
	{
		return amiphod switch
		{
			'A' => 1,
			'B' => 10,
			'C' => 100,
			'D' => 1000,
			_ => throw new NotImplementedException()
		};
	}

	private IEnumerable<int> GetAccessibleHallways(Room fromRoom)
	{
		for (int i = fromRoom.hallwayIndex; i >= 0; i--)
		{
			if (hallway[i] != null) break;
			if (!RoomIndexes.Contains(i)) yield return i;
		}

		for (int i = fromRoom.hallwayIndex; i < hallway.Count; i++)
		{
			if (hallway[i] != null) break;
			if (!RoomIndexes.Contains(i)) yield return i;
		}
	}

	public string GetStateString()
	{
		var builder = new StringBuilder();

		foreach (var h in hallway)
		{
			builder.Append(h ?? ' ');
		}

		foreach (var r in rooms)
		{
			builder.Append(r.pos1 ?? ' ');
			builder.Append(r.pos2 ?? ' ');
		}

		return builder.ToString();
	}

}