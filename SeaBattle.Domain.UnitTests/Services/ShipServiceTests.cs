using FluentAssertions;
using Moq.AutoMock;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services;
using SeaBattle.Domain.UnitTests.Common;
using Xunit;

namespace SeaBattle.Domain.UnitTests.Services
{
    public class ShipServiceTests
    {
        [Fact]
        public void ShipSafetyZone_WhenShipOccupiesTwoFields_ThenTenPointsAroundShouldBeReturned()
        {
            // Arrange
            var (ship, pointsAround) = ShipWithPointsAround();

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            var safetyZone = subject.ShipSafetyZone(ship);

            // Assert
            safetyZone.Should().BeEquivalentTo(pointsAround);
        }

        [Fact]
        public void IsAllShipsDestroyed_WhenAllShipsKilled_ThenReturnTrue()
        {
            // Arrange
            var ship1 = new[] { new Point(0, 0), new Point(1, 0) };
            var ship2 = new[] { new Point(2, 0) };

            var map = new Map(3);
            map.AddShip(ship1);
            map.AddShip(ship2);
            foreach (var point in ship1.Concat(ship2))
            {
                map.SetField(point, FieldType.KilledBoat);
            }

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            var result = subject.IsAllShipsDestroyed(map);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsAllShipsDestroyed_WhenNotAllShipsKilled_ThenReturnFalse()
        {
            // Arrange
            var ship1 = new[] { new Point(0, 0), new Point(1, 0) };
            var ship2 = new[] { new Point(2, 0) };

            var map = new Map(3);
            map.AddShip(ship1);
            map.AddShip(ship2);
            foreach (var point in ship1)
            {
                map.SetField(point, FieldType.KilledBoat);
            }

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            var result = subject.IsAllShipsDestroyed(map);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void DamageShip_WhenHitPointOutsideOfShip_ThenMapIsNotChanged()
        {
            // Arrange
            var ship = new[] { new Point(2, 0) };

            var map = new Map(3);       
            map.AddShip(ship);
            var initialFields = map.EnumerateFields().ToArray();

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            subject.DamageShip(map, new Point(1, 0));

            // Assert
            MapValidationHelpers.ValidateFieldsTheSame(initialFields, map.EnumerateFields().ToArray());
        }

        [Fact]
        public void DamageShip_WhenDamageOneSectionOfShip_ThenDamagedBoatFieldShouldAppearOnThatPointAndOtherPointsUnchanged()
        {
            // Arrange
            var ship = new[] { new Point(2, 0), new Point(2, 1) };

            var map = new Map(5);
            map.AddShip(ship);
            var initialFields = map.EnumerateFields().ToArray();

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            subject.DamageShip(map, ship[0]);

            // Assert
            map.GetField(ship[0]).Should().Be(FieldType.DamagedBoat);
            MapValidationHelpers.ValidateFieldsTheSame(initialFields, map.EnumerateFields().ToArray(), ship[0]);
        }

        [Fact]
        public void DamageShip_WhenDamageTwoSectionOfTwoSectionShip_ThenShipShouldBeMarkedAsKilled()
        {
            // Arrange
            var ship = new[] { new Point(2, 0), new Point(2, 1) };

            var map = new Map(5);
            map.AddShip(ship);

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            subject.DamageShip(map, ship[0]);
            subject.DamageShip(map, ship[1]);

            // Assert
            map.GetField(ship[0]).Should().Be(FieldType.KilledBoat);
            map.GetField(ship[1]).Should().Be(FieldType.KilledBoat);
        }

        [Fact]
        public void DamageShip_WhenDamageTwoSectionsOfTwoSectionShip_ThenSafetyZoneAroundShouldBeMarkedAsChecked()
        {
            // Arrange
            var (ship, pointsAround) = ShipWithPointsAround();

            var map = new Map(5);
            map.AddShip(ship);

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            subject.DamageShip(map, ship[0]);
            subject.DamageShip(map, ship[1]);

            // Assert
            pointsAround.Select(map.GetField).Should().AllSatisfy(type => type.Should().Be(FieldType.CheckedSee));
        }

        [Fact]
        public void DamageShip_WhenKillTwoSectionShip_ThenFieldsOutsideShipAndSafetyZoneShouldRemainTheSame()
        {
            // Arrange
            var (ship, pointsAround) = ShipWithPointsAround();

            var map = new Map(5);
            map.AddShip(ship);
            var initialFields = map.EnumerateFields().ToArray();

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<ShipService>();

            // Act
            subject.DamageShip(map, ship[0]);
            subject.DamageShip(map, ship[1]);

            // Assert
            MapValidationHelpers.ValidateFieldsTheSame(
                initialFields, 
                map.EnumerateFields().ToArray(),
                pointsAround.Concat(ship).ToArray());
        }

        private static (Point[], Point[]) ShipWithPointsAround()
        {
            var ship = new Point[] { new(1, 1), new(2, 1) };
            var pointsAround = new Point[]
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0),
                new(0, 1),
                new(3, 1),
                new(0, 2),
                new(1, 2),
                new(2, 2),
                new(3, 2),
            };

            return (ship, pointsAround);
        }
    }
}