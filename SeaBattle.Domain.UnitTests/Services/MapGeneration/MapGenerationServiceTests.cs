using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.AutoMock;
using SeaBattle.Domain.Configuration;
using SeaBattle.Domain.Exceptions;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.MapGeneration;
using SeaBattle.Domain.Services.MapGeneration.Interfaces;
using Xunit;

namespace SeaBattle.Domain.UnitTests.Services.MapGeneration
{
    public class MapGenerationServiceTests
    {
        [Theory]
        [InlineData(3)]
        [InlineData(8)]
        [InlineData(10)]
        public void GenerateRandomMapWithShips_WhenMapSizeConfigurationHasValue_ThenMapWithDefinedSizeHasToBeCreated(int mapSize)
        {
            // Arrange
            var gameConfiguration = new GameConfiguration
            {
                MapSize = mapSize,
                ShipTypes = Array.Empty<int>()
            };

            var mocker = new AutoMocker();
            mocker.GetMock<IOptions<GameConfiguration>>()
                .Setup(options => options.Value)
                .Returns(gameConfiguration);

            var subject = mocker.CreateInstance<MapGenerationService>();

            // Act
            var map = subject.GenerateRandomMapWithShips();

            // Assert
            map.Size.Should().Be(mapSize);
        }

        [Fact]
        public void GenerateRandomMapWithShips_WhenTwoFieldShipIsExpected_ThenMapWithTwoFieldShipHasToBeCreated()
        {
            // Arrange
            var expectedShipPosition = new Point[] { new(1, 1), new(2, 1) };
            var gameConfiguration = new GameConfiguration
            {
                MapSize = 3,
                ShipTypes = new[] { 2 }
            };

            var mocker = new AutoMocker();

            mocker.GetMock<IOptions<GameConfiguration>>()
                .Setup(options => options.Value)
                .Returns(gameConfiguration);
            mocker.GetMock<IShipPlacementService>()
                .Setup(service => service.FindRandomPosition(It.IsAny<int>(), 2))
                .Returns(expectedShipPosition);
            mocker.GetMock<IShipPlacementService>()
                .Setup(service => service.ValidateShipPosition(It.IsAny<Map>(), It.IsAny<Point[]>()))
                .Returns(true);

            var subject = mocker.CreateInstance<MapGenerationService>();

            // Act
            var map = subject.GenerateRandomMapWithShips();

            // Assert
            expectedShipPosition.Select(map.GetField).Should().AllSatisfy(type => type.Should().Be(FieldType.Boat));
        }

        [Fact]
        public void GenerateRandomMapWithShips_WhenTwoFieldShipIsExpectedButNotAbleToPlace_ThenExceptionHasToBeRaised()
        {
            // Arrange
            var expectedShipPosition = new Point[] { new(1, 1), new(2, 1) };
            var gameConfiguration = new GameConfiguration
            {
                MapSize = 3,
                ShipTypes = new[] { 2 },
                MapGeneratorMaxNumberOfRetries = 1
            };

            var mocker = new AutoMocker();

            mocker.GetMock<IOptions<GameConfiguration>>()
                .Setup(options => options.Value)
                .Returns(gameConfiguration);
            mocker.GetMock<IShipPlacementService>()
                .Setup(service => service.FindRandomPosition(It.IsAny<int>(), 2))
                .Returns(expectedShipPosition);
            mocker.GetMock<IShipPlacementService>()
                .Setup(service => service.ValidateShipPosition(It.IsAny<Map>(), It.IsAny<Point[]>()))
                .Returns(false);

            var subject = mocker.CreateInstance<MapGenerationService>();

            // Act && Assert
            subject.Invoking(service => service.GenerateRandomMapWithShips()).Should().Throw<GameException>();
        }
    }
}