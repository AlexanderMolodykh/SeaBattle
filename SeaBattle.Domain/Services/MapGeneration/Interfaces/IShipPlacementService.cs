using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Services.MapGeneration.Interfaces;

public interface IShipPlacementService
{
    /// <summary>
    /// Searches random position for the ship to be placed.
    /// </summary>
    /// <returns>Array of point that represents ship position.</returns>
    Point[] FindRandomPosition(int mapSize, int shipSize);

    /// <summary>
    /// Checks whether ship is allowed to be placed at that point.
    /// </summary>
    /// <param name="ship">Array of point that represents ship position.</param>
    /// <returns>True if ship is allowed to be placed.</returns>
    bool ValidateShipPosition(Map map, Point[] ship);
}