using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.MapGeneration;
using Xunit;

namespace SeaBattle.Domain.UnitTests.Services.MapGeneration
{
    public class ShipPlacementServiceTests
    {
        [Fact]
        public void FindRandomPosition_WhenExpectedShipIsHorizontal_ThenListOfPointsShouldRepresentPositionOfThatShip()
        {
            // Arrange
            var mapSize = 5;
            var shipSize = 2;
            var expectedShip = new Point[]
            {
                new(1, 2),
                new(2, 2)
            };
            var isHorizontalRandomizingEquivalent = 0;

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 2))
                .Returns(isHorizontalRandomizingEquivalent);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 5))
                .Returns(2);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 3))
                .Returns(1);

            var subject = mocker.CreateInstance<ShipPlacementService>();
            // Act
            var ship = subject.FindRandomPosition(mapSize, shipSize);

            // Assert
            ship.Should().BeEquivalentTo(expectedShip);
        }
        
        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 0)]
        [InlineData(0, 4)]
        [InlineData(2, 4)]
        public void FindRandomPosition_WhenExpectedShipIsHorizontal_ThenListOfPointsShouldNeverExceedMapFieldsRange(int x,int y)
        {
            // Arrange
            var mapSize = 5;
            var shipSize = 2;
            var isHorizontalRandomizingEquivalent = 0;

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 2))
                .Returns(isHorizontalRandomizingEquivalent);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 3))
                .Returns(x);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 5))
                .Returns(y);

            var subject = mocker.CreateInstance<ShipPlacementService>();

            // Act
            var ship = subject.FindRandomPosition(mapSize, shipSize);

            // Assert
            ship.Should().AllSatisfy(point => point.x.Should().BeInRange(0, mapSize - 1));
            ship.Should().AllSatisfy(point => point.y.Should().BeInRange(0, mapSize - 1));
        }

        [Fact]
        public void FindRandomPosition_WhenExpectedShipIsVertical_ThenListOfPointsShouldRepresentPositionOfThatShip()
        {
            // Arrange
            var mapSize = 5;
            var shipSize = 2;
            var expectedShip = new Point[]
            {
                new(2, 1),
                new(2, 2)
            };
            var isVerticalRandomizingEquivalent = 1;

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 2))
                .Returns(isVerticalRandomizingEquivalent);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 5))
                .Returns(2);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 3))
                .Returns(1);

            var subject = mocker.CreateInstance<ShipPlacementService>();

            // Act
            var ship = subject.FindRandomPosition(mapSize, shipSize);

            // Assert
            ship.Should().BeEquivalentTo(expectedShip);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 2)]
        [InlineData(4, 0)]
        [InlineData(4, 2)]
        public void FindRandomPosition_WhenExpectedShipIsVertical_ThenListOfPointsShouldNeverExceedMapFieldsRange(int x, int y)
        {
            // Arrange
            var mapSize = 5;
            var shipSize = 2;
            var isVerticalRandomizingEquivalent = 1;

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 2))
                .Returns(isVerticalRandomizingEquivalent);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 5))
                .Returns(x);
            mocker.GetMock<IRandomService>()
                .Setup(service => service.Next(0, 3))
                .Returns(y);

            var subject = mocker.CreateInstance<ShipPlacementService>();

            // Act
            var ship = subject.FindRandomPosition(mapSize, shipSize);

            // Assert
            ship.Should().AllSatisfy(point => point.x.Should().BeInRange(0, mapSize - 1));
            ship.Should().AllSatisfy(point => point.y.Should().BeInRange(0, mapSize - 1));
        }

        [Fact]
        public void ValidateShipPosition_WhenPlaceUnderShipAndSafetyZoneDoesntHaveAnyShip_ThenIsValidShipPosition()
        {
            // Arrange
            var map = new Map(5);
            var ship = new Point[]
            {
                new(1, 1),
                new(2, 1)
            };
            var safetyZone = new Point[]
            {
                new(1, 0),
                new(2, 0)
            };


            var mocker = new AutoMocker();
            mocker.GetMock<IShipService>()
                .Setup(service => service.ShipSafetyZone(It.IsAny<Point[]>()))
                .Returns(safetyZone);
            var subject = mocker.CreateInstance<ShipPlacementService>();

            // Act
            var validationResult = subject.ValidateShipPosition(map, ship);

            // Assert
            validationResult.Should().BeTrue();
        }

        [Theory]
        [InlineData(FieldType.Boat, false)]
        [InlineData(FieldType.See, true)]
        [InlineData(FieldType.MapBorder, false)]
        public void ValidateShipPosition_WhenPlaceUnderShipHasSomeFieldAlreadyAndSafetyZoneIsValid_ThenExpectedValidationResultShouldBeReturned(FieldType existingFieldType, bool expectedValidationResult)
        {
            // Arrange
            var map = new Map(5);
            var ship = new Point[]
            {
                new(1, 1),
                new(2, 1)
            };
            var safetyZone = new Point[]
            {
                new(1, 0),
                new(2, 0)
            };

            map.SetField(ship.First(), existingFieldType);

            var mocker = new AutoMocker();
            mocker.GetMock<IShipService>()
                .Setup(service => service.ShipSafetyZone(It.IsAny<Point[]>()))
                .Returns(safetyZone);
            var subject = mocker.CreateInstance<ShipPlacementService>();

            // Act
            var result = subject.ValidateShipPosition(map, ship);

            // Assert
            result.Should().Be(expectedValidationResult);
        }

        [Theory]
        [InlineData(FieldType.Boat, false)]
        [InlineData(FieldType.See, true)]
        [InlineData(FieldType.MapBorder, true)]
        public void ValidateShipPosition_WhenSafetyZoneAlreadyHasCertainExistingFieldTypeAndShipPlacementIsValid_ThenExpectedValidationResultShouldBeReturned(FieldType existingFieldType, bool expectedValidationResult)
        {
            // Arrange
            var map = new Map(5);
            var ship = new Point[]
            {
                new(1, 1),
                new(2, 1)
            };
            var safetyZone = new Point[]
            {
                new(1, 0),
                new(2, 0)
            };

            map.SetField(safetyZone.First(), existingFieldType);

            var mocker = new AutoMocker();
            mocker.GetMock<IShipService>()
                .Setup(service => service.ShipSafetyZone(It.IsAny<Point[]>()))
                .Returns(safetyZone);
            var subject = mocker.CreateInstance<ShipPlacementService>();

            // Act
            var result = subject.ValidateShipPosition(map, ship);

            // Assert
            result.Should().Be(expectedValidationResult);
        }
    }
}