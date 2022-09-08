using FluentAssertions;
using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.UnitTests.Common
{
    public class MapValidationHelpers
    {

        public static void ValidateFieldsTheSame(MapPoint[] fields1, MapPoint[] fields2, params Point[] pointsToExclude)
        {
            var filteredFields1 = fields1.Where(mapPoint => !pointsToExclude.Contains(mapPoint.point));
            var filteredFields2 = fields2.Where(mapPoint => !pointsToExclude.Contains(mapPoint.point));
            filteredFields1.Should().BeEquivalentTo(filteredFields2);
        }
    }

    public class MapCreationHelpers
    {
        public static Map CreateMapWithDamagedShip(Point damagedPoint, out Point[] damagedShip)
        {
            var map = new Map(5);
            map.AddShip(new Point[]
            {
                new(1, 0),
                new(1, 1),
                new(1, 2),
                new(1, 3),
            });
            map.SetField(damagedPoint, FieldType.DamagedBoat);
            damagedShip = map.GetShips().First();
            return map;
        }


    }
}
