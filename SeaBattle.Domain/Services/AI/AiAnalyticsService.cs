using SeaBattle.Domain.Extensions;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.AI.Interfaces;

namespace SeaBattle.Domain.Services.AI
{
    public class AiAnalyticsService : IAiAnalyticsService
    {
        public IEnumerable<Point> GetPossiblePlacesToFire(Map map, Point[] ship)
        {
            var damagedFields = map.EnumerateFields(ship)
                .Where(mapPoint => mapPoint.field == FieldType.DamagedBoat)
                .ToList();

            if (damagedFields.Count == 1)
            {
                return EnumPointsAround(map, damagedFields.First())
                    .Where(point => point.field.IsAllowedToFire())
                    .Select(point => point.point);
            }

            // todo if more points exclude side points
            return damagedFields.SelectMany(point => EnumPointsAround(map, point))
                .Where(point => point.field.IsAllowedToFire())
                .Select(point => point.point);
        }

        private static IEnumerable<MapPoint> EnumPointsAround(Map map, MapPoint mapPoint)
        {
            var centralPoint = mapPoint.point;
            var pointsAround = new List<Point>
            {
                new(centralPoint.x - 1, centralPoint.y),
                new(centralPoint.x + 1, centralPoint.y),
                new(centralPoint.x, centralPoint.y - 1),
                new(centralPoint.x, centralPoint.y + 1),
            };

            return map.EnumerateFields(pointsAround);
        }
    }
}
