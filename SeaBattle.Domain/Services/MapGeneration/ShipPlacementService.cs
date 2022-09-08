using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.MapGeneration.Interfaces;

namespace SeaBattle.Domain.Services.MapGeneration
{
    public class ShipPlacementService : IShipPlacementService
    {
        private readonly IRandomService _randomService;
        private readonly IShipService _shipService;

        public ShipPlacementService(IRandomService randomService, IShipService shipService)
        {
            _randomService = randomService;
            _shipService = shipService;
        }

        public Point[] FindRandomPosition(int mapSize, int shipSize)
        {
            var isHorizontal = _randomService.Next(0, 2) == 0;
            var positionX = _randomService.Next(0, mapSize - (isHorizontal ? shipSize : 0));
            var positionY = _randomService.Next(0, mapSize - (isHorizontal ? 0 : shipSize));

            if (isHorizontal)
            {
                return Enumerable.Range(positionX, shipSize).Select(x => new Point(x, positionY)).ToArray();
            }
            return Enumerable.Range(positionY, shipSize).Select(y => new Point(positionX, y)).ToArray();
        }

        public bool ValidateShipPosition(Map map, Point[] ship)
        {
            if (!IsSeaOnly(map, ship))
            {
                return false;
            }

            var shipSafeZone = _shipService.ShipSafetyZone(ship);
            return IsSeaOrMapBorderOnly(map, shipSafeZone);
        }

        private static bool IsSeaOnly(Map map, IEnumerable<Point> points)
        {
            return points.Select(map.GetField).All(type => type == FieldType.See);
        }

        private static bool IsSeaOrMapBorderOnly(Map map, IEnumerable<Point> points)
        {
            return points.Select(map.GetField).All(type => type is FieldType.See or FieldType.MapBorder);
        }
    }
}