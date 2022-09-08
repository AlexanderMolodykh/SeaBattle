using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Services
{
    public class ShipService : IShipService
    {
        public IEnumerable<Point> ShipSafetyZone(Point[] damagedShip)
        {
            var start = damagedShip.First();
            var end = damagedShip.Last();
            for (var x = start.x - 1; x <= end.x + 1; x++)
            {
                for (var y = start.y - 1; y <= end.y + 1; y++)
                {
                    var point = new Point(x, y);

                    if (!damagedShip.Contains(point))
                    {
                        yield return point;
                    }
                }
            }
        }

        public bool IsAllShipsDestroyed(Map map)
        {
            return map.GetShips().All(points => IsShipDestroyed(map, points));
        }

        public void DamageShip(Map map, Point point)
        {
            var damagedShip = map.GetShips().FirstOrDefault(ship => ship.Contains(point));

            if (damagedShip == null)
            {
                return;
            }

            map.SetField(point, FieldType.DamagedBoat);

            if (!IsShipDestroyed(map, damagedShip))
            {
                return;
            }

            MarkShipAsKilled(map, damagedShip);
        }

        private void MarkShipAsKilled(Map map, Point[] damagedShip)
        {
            var safetyZone = ShipSafetyZone(damagedShip);
            
            foreach (var point in safetyZone)
            {
                map.SetField(point, FieldType.CheckedSee);
            }
            
            foreach (var point in damagedShip)
            {
                map.SetField(point, FieldType.KilledBoat);
            }
        }

        private static bool IsShipDestroyed(Map map, Point[] damagedShip)
        {
            return damagedShip
                .Select(map.GetField)
                .All(type => type != FieldType.Boat);
        }
    }
}
