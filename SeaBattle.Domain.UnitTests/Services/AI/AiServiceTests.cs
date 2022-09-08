using FluentAssertions;
using Moq;
using Moq.AutoMock;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.AI;
using SeaBattle.Domain.Services.AI.Interfaces;
using SeaBattle.Domain.UnitTests.Common;
using Xunit;

namespace SeaBattle.Domain.UnitTests.Services.AI
{
    public class AiServiceTests
    {
        [Fact]
        public void GetPossibleMove_WhenNoDamagedShips_ThenForDifferentRandomGeneratorNumberDifferentPointsShouldBeReturned()
        {
            // Arrange
            var map = new Map(5);

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .SetupSequence(options => options.Next(It.IsAny<int>(),It.IsAny<int>()))
                .Returns(3)
                .Returns(2);

            var subject = mocker.CreateInstance<AiService>();

            // Act
            var possiblePlacesToFire1 = subject.GetPossibleMove(map);
            var possiblePlacesToFire2 = subject.GetPossibleMove(map);

            // Assert
            possiblePlacesToFire1.Should().NotBe(possiblePlacesToFire2);
        }

        [Fact]
        public void GetPossibleMove_WhenNoDamagedShips_ThenForSameRandomGeneratorNumberSamePointsShouldBeReturned()
        {
            // Arrange
            var map = new Map(5);

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(options => options.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(3);

            var subject = mocker.CreateInstance<AiService>();

            // Act
            var possiblePlacesToFire1 = subject.GetPossibleMove(map);
            var possiblePlacesToFire2 = subject.GetPossibleMove(map);

            // Assert
            possiblePlacesToFire1.Should().Be(possiblePlacesToFire2);
        }

        [Fact]
        public void GetPossibleMove_WhenNoDamagedShipsAndTargetedFieldIsAvailable_ThenReturnIt()
        {
            // Arrange
            var map = new Map(5);

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(options => options.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var subject = mocker.CreateInstance<AiService>();

            // Act
            var possiblePlacesToFire = subject.GetPossibleMove(map);

            // Assert
            possiblePlacesToFire.Should().Be(new Point(0, 0));
        }

        [Fact]
        public void GetPossibleMove_WhenNoDamagedShipsAndTargetedFieldIsNotAvailable_ThenReturnNext()
        {
            // Arrange
            var map = new Map(5);
            map.SetField(new Point(0, 0), FieldType.CheckedSee);

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(options => options.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            var subject = mocker.CreateInstance<AiService>();

            // Act
            var possiblePlacesToFire = subject.GetPossibleMove(map);

            // Assert
            possiblePlacesToFire.Should().Be(new Point(0, 1));
        }

        [Fact]
        public void GetPossibleMove_WhenNoDamagedShipsAndTargetedFieldIsInTheEndOfMapAndNotAvailable_ThenReturnNexFromBeginningt()
        {
            // Arrange
            var map = new Map(3);
            map.SetField(new Point(2, 2), FieldType.CheckedSee);

            var mocker = new AutoMocker();
            mocker.GetMock<IRandomService>()
                .Setup(options => options.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(8);

            var subject = mocker.CreateInstance<AiService>();

            // Act
            var possiblePlacesToFire = subject.GetPossibleMove(map);

            // Assert
            possiblePlacesToFire.Should().Be(new Point(0, 0));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetPossibleMove_WhenDamagedShips_ThenOneOfThePossiblePlacesToFireAroundDamageHasToBeReturned(int randomValue)
        {
            // Arrange
            var damagedPoint = new Point(1, 1);
            var map = MapCreationHelpers.CreateMapWithDamagedShip(damagedPoint, out var _);
            var expectedPoints = new[] { new Point(1, 1), new Point(2, 2) };

            var mocker = new AutoMocker();
            mocker.GetMock<IAiAnalyticsService>()
                .Setup(options => options.GetPossiblePlacesToFire(It.IsAny<Map>(), It.IsAny<Point[]>()))
                .Returns(expectedPoints);
            mocker.GetMock<IRandomService>()
                .Setup(options => options.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(randomValue);

            var subject = mocker.CreateInstance<AiService>();

            // Act
            var possiblePlacesToFire = subject.GetPossibleMove(map);

            // Assert
            possiblePlacesToFire.Should().Be(expectedPoints[randomValue]);
        }
    }
}