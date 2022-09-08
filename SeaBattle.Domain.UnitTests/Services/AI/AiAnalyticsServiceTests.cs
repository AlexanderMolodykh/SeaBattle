using FluentAssertions;
using Moq.AutoMock;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.AI;
using SeaBattle.Domain.UnitTests.Common;
using Xunit;

namespace SeaBattle.Domain.UnitTests.Services.AI
{
    public class AiAnalyticsServiceTests
    {
        [Fact]
        public void GetPossiblePlacesToFire_WhenShipHasOneSectionDamaged_ThenReturnPointsAroundThatSection()
        {
            // Arrange
            var damagedPoint = new Point(1, 1);
            var map = MapCreationHelpers.CreateMapWithDamagedShip(damagedPoint, out var damagedShip);

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<AiAnalyticsService>();

            // Act
            var possiblePlacesToFire = subject.GetPossiblePlacesToFire(map, damagedShip);

            // Assert
            possiblePlacesToFire.Should().HaveCount(4);
            possiblePlacesToFire.Should().Contain(new Point(0, 1));
            possiblePlacesToFire.Should().Contain(new Point(2, 1));
            possiblePlacesToFire.Should().Contain(new Point(1, 0));
            possiblePlacesToFire.Should().Contain(new Point(1, 2));
        }

        [Fact]
        public void GetPossiblePlacesToFire_WhenShipHasDamagePointNearTheEdge_ThenDoNotReturnEdgeFields()
        {
            // Arrange
            var damagedPoint = new Point(1, 0);
            var map = MapCreationHelpers.CreateMapWithDamagedShip(damagedPoint, out var damagedShip);

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<AiAnalyticsService>();

            // Act
            var possiblePlacesToFire = subject.GetPossiblePlacesToFire(map, damagedShip);

            // Assert
            possiblePlacesToFire.Should().HaveCount(3);
            possiblePlacesToFire.Should().NotContain(new Point(1, -1));
        }

        [Fact]
        public void GetPossiblePlacesToFire_WhenShipHasCheckedFieldAround_ThenDoNotReturnCheckedFields()
        {
            var damagedPoint = new Point(1, 1);
            var map = MapCreationHelpers.CreateMapWithDamagedShip(damagedPoint, out var damagedShip);
            map.SetField(new Point(0, 1), FieldType.CheckedSee);

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<AiAnalyticsService>();

            // Act
            var possiblePlacesToFire = subject.GetPossiblePlacesToFire(map, damagedShip);

            // Assert
            possiblePlacesToFire.Should().HaveCount(3);
            possiblePlacesToFire.Should().NotContain(new Point(0, 1));
        }

        [Fact]
        public void GetPossiblePlacesToFire_WhenShipHasDamagedFieldAround_ThenDoNotReturnDamagedFields()
        {
            var damagedPoint = new Point(1, 1);
            var map = MapCreationHelpers.CreateMapWithDamagedShip(damagedPoint, out var damagedShip);
            map.SetField(new Point(0, 1), FieldType.DamagedBoat);

            var mocker = new AutoMocker();
            var subject = mocker.CreateInstance<AiAnalyticsService>();

            // Act
            var possiblePlacesToFire = subject.GetPossiblePlacesToFire(map, damagedShip);

            // Assert
            possiblePlacesToFire.Should().HaveCount(3);
            possiblePlacesToFire.Should().NotContain(new Point(0, 1));
        }
    }
}