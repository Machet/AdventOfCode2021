var (width, height, east, south) = ParseMap(File.ReadAllLines("input.txt"));

var step = 1;
while (true)
{
    var newEast = east.Select(c => c.MoveEast(width, south, east)).ToHashSet();
    var hasMovedEast = newEast.Except(east).Any();
    east = newEast;

    var newSouth = south.Select(c => c.MoveSouth(height, south, east)).ToHashSet();
    var hasMovedSouth = newSouth.Except(south).Any();
    south = newSouth;

    if (!hasMovedEast && !hasMovedSouth)
    {
        Console.WriteLine(step);
        break;
    }

    step++;
}

static (int width, int height, HashSet<Position> east, HashSet<Position> south) ParseMap(string[] lines)
{
    var east = new HashSet<Position>();
    var south = new HashSet<Position>();

    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[i].Length; j++)
        {
            if (lines[i][j] == '>')
            {
                east.Add(new Position(i, j));
            }

            if (lines[i][j] == 'v')
            {
                south.Add(new Position(i, j));
            }
        }
    }

    return (lines[0].Length, lines.Length, east, south);
}

record Position(int x, int y)
{
    public Position MoveEast(int max, HashSet<Position> east, HashSet<Position> south)
    {
        var pos = new Position(x, (y + 1) % max);
        return east.Contains(pos) || south.Contains(pos) ? this : pos;

    }

    public Position MoveSouth(int max, HashSet<Position> east, HashSet<Position> south)
    {
        var pos = new Position((x + 1) % max, y);
        return east.Contains(pos) || south.Contains(pos) ? this : pos;

    }
}