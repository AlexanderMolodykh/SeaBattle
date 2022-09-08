using SeaBattle.Domain.Extensions;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.AI.Interfaces;

namespace SeaBattle.Domain.Services.AI;

public class AiService : IAiService
{
    private readonly IRandomService _randomService;
    private readonly IAiAnalyticsService _aiAnalyticsService;

    public AiService(IRandomService randomService, IAiAnalyticsService aiAnalyticsService)
    {
        _randomService = randomService;
        _aiAnalyticsService = aiAnalyticsService;
    }

    public Point GetPossibleMove(Map map)
    {
        var result = FindDamagedShipAndTryToDestroy(map);
        if (result.HasValue)
        {
            return result.Value;
        }

        return FindRandomAvailablePlaceToFire(map);
    }

    private Point FindRandomAvailablePlaceToFire(Map map)
    {
        var randomShoot = _randomService.Next(0, map.Size * map.Size);
        var randomCandidates = map.EnumerateFields().Skip(randomShoot).Concat(map.EnumerateFields());
        var findRandomAvailablePlaceToFire = randomCandidates.First(mapPoint => mapPoint.field.IsAllowedToFire());

        return findRandomAvailablePlaceToFire.point;
    }

    private Point? FindDamagedShipAndTryToDestroy(Map map)
    {
        var points = FindDamagedShipAndPossiblePlacesToFire(map);
        if (!points.Any())
        {
            return null;
        }

        var randomPlaceToFire = _randomService.Next(0, points.Length);
        return points[randomPlaceToFire];
    }

    private Point[] FindDamagedShipAndPossiblePlacesToFire(Map map)
    {
        foreach (var ship in EnumerateDamagedShips(map))
        {
            return _aiAnalyticsService.GetPossiblePlacesToFire(map, ship).ToArray();
        }

        return Array.Empty<Point>();
    }

    private static IEnumerable<Point[]> EnumerateDamagedShips(Map map)
    {
        return map.GetShips()
            .Where(ship => ship
                .Select(map.GetField)
                .Contains(FieldType.DamagedBoat));
    }
}