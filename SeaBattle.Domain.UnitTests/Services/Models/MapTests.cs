using FluentAssertions;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.UnitTests.Common;
using Xunit;

namespace SeaBattle.Domain.UnitTests.Services.Models
{
    public class MapTests
    {
        [Fact]
        public void EnumerateFields_WhenMapHasSizeThree_ThenAmountOfFieldShouldBeNine()
        {
            // Arrange
            var map = new Map(3);

            // Act
            var enumerateFields = map.EnumerateFields();

            // Assert
            enumerateFields.Should().HaveCount(9);
        }

        [Fact]
        public void EnumerateFields_WhenEnumeratingWholeMap_ThenAllFieldsShouldBeEnumerated()
        {
            // Arrange
            var map = new Map(2);

            // Act
            var enumerateFields = map.EnumerateFields();

            // Assert
            enumerateFields.Should().Contain(point => point.point == new Point(0, 0));
            enumerateFields.Should().Contain(point => point.point == new Point(0, 1));
            enumerateFields.Should().Contain(point => point.point == new Point(1, 0));
            enumerateFields.Should().Contain(point => point.point == new Point(1, 1));
        }

        [Fact]
        public void EnumerateFields_WhenEnumeratingPartOfMap_ThenRequestedFieldsShouldBeEnumerated()
        {
            // Arrange
            var map = new Map(2);

            // Act
            var enumerateFields = map.EnumerateFields(new[] { new Point(0, 1), new Point(1, 1) });

            // Assert
            enumerateFields.Should().Contain(point => point.point == new Point(0, 1));
            enumerateFields.Should().Contain(point => point.point == new Point(1, 1));
        }

        [Fact]
        public void EnumerateFields_WhenNewlyCreatedMap_ThenAllFieldsHaveSeeType()
        {
            // Arrange
            var map = new Map(3);

            // Act
            var enumerateFields = map.EnumerateFields();

            // Assert
            enumerateFields.Should()
                .AllSatisfy(point => point.field.Should().Be(FieldType.See));
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(4, 4)]
        [InlineData(0, 4)]
        [InlineData(4, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-20, -20)]
        [InlineData(20, 20)]
        public void GetField_WhenRequestedPointOutsideOfGameField_ThenReturnMapBorderFieldType(int x, int y)
        {
            // Arrange
            var map = new Map(3);

            // Act
            var fieldType = map.GetField(new Point(x, y));

            // Assert
            fieldType.Should().Be(FieldType.MapBorder);
        }

        [Theory]
        [InlineData(-1, -1)]
        [InlineData(4, 4)]
        [InlineData(0, 4)]
        [InlineData(4, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-20, -20)]
        [InlineData(20, 20)]
        public void SetField_WhenRequestedPointOutsideOfGameField_ThenDoNotThrowExceptions(int x, int y)
        {
            // Arrange
            var map = new Map(3);

            // Act
            Action action = () => map.SetField(new Point(x, y), FieldType.Boat);

            // Assert
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(FieldType.See)]
        [InlineData(FieldType.Boat)]
        [InlineData(FieldType.CheckedSee)]
        [InlineData(FieldType.DamagedBoat)]
        [InlineData(FieldType.KilledBoat)]
        public void SetField_WhenChangingField_ThenThisFieldShouldBeChanged(FieldType fieldType)
        {
            // Arrange
            var map = new Map(3);
            var point = new Point(1, 1);

            // Act
            map.SetField(point , fieldType);

            // Assert
            map.GetField(point).Should().Be(fieldType);
        }

        [Theory]
        [InlineData(FieldType.See)]
        [InlineData(FieldType.Boat)]
        [InlineData(FieldType.CheckedSee)]
        [InlineData(FieldType.DamagedBoat)]
        [InlineData(FieldType.KilledBoat)]
        public void SetField_WhenChangingField_ThenOtherFieldsShouldBeChanged(FieldType fieldType)
        {
            // Arrange
            var map = new Map(3);
            var point = new Point(1, 1);
            var initialFields = map.EnumerateFields().ToArray();

            // Act
            map.SetField(point , fieldType);

            // Assert
            MapValidationHelpers.ValidateFieldsTheSame(initialFields, map.EnumerateFields().ToArray(), point);
        }

        [Theory]
        [InlineData(FieldType.See)]
        [InlineData(FieldType.Boat)]
        [InlineData(FieldType.CheckedSee)]
        [InlineData(FieldType.DamagedBoat)]
        [InlineData(FieldType.KilledBoat)]
        public void SetField_WhenChangeFieldTypeAtDefinedPoint_ThenThisPointShouldContainThatField(FieldType fieldType)
        {
            // Arrange
            var map = new Map(3);
            var point = new Point(1, 1);

            // Act
            map.SetField(point , fieldType);

            // Assert
            map.GetField(point).Should().Be(fieldType);
        }

        [Fact]
        public void AddShip_WhenShipAdded_ThenCorrespondingFieldMarkedAsBoatAndOtherAreSee()
        {
            // Arrange
            var map = new Map(3);
            var shipPoint = new Point(1, 1);

            // Act
            map.AddShip(new[] { shipPoint });

            // Assert
            map.GetField(shipPoint).Should().Be(FieldType.Boat);
            map.EnumerateFields().Where(point => point.point != shipPoint)
                .Should().AllSatisfy(point => point.field.Should().Be(FieldType.See));
        }
    }
}