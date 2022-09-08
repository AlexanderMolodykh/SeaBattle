namespace SeaBattle.Domain.Models;

public class Map
{
    private readonly FieldType[,] _fields;
    private readonly List<Point[]> _ships;

    public Map(int size)
    {
        _fields = new FieldType[size, size];
        _ships = new List<Point[]>();
        Size = size;
    }

    public int Size { get; }

    public FieldType GetField(Point point)
    {
        if (point.x < 0 || point.x >= Size || point.y < 0 || point.y >= Size)
        {
            return FieldType.MapBorder;
        }

        return _fields[point.x, point.y];
    }

    public void SetField(Point point, FieldType field)
    {
        if (point.x < 0 || point.x >= Size || point.y < 0 || point.y >= Size)
        {
            return;
        }

        _fields[point.x, point.y] = field;
    }

    public IEnumerable<MapPoint> EnumerateFields()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                yield return new MapPoint(new Point(x, y), _fields[x, y]);
            }
        }
    }

    public IEnumerable<MapPoint> EnumerateFields(IEnumerable<Point> points)
    {
        foreach (var point in points)
        {
            yield return new MapPoint(point, GetField(point));
        }
    }

    public void AddShip(Point[] shipPoints)
    {
        _ships.Add(shipPoints);

        foreach (var point in shipPoints)
        {
            SetField(point, FieldType.Boat);
        }
    }

    public IReadOnlyCollection<Point[]> GetShips()
    {
        return _ships;
    }
}