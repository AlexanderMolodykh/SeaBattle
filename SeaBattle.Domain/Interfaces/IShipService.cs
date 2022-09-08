using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Interfaces;

public interface IShipService
{
    /// <summary>
    /// Defines safety zone around a ship where other ships are not allowed to be.
    /// </summary>
    /// <param name="ship">The list of points that defines ship.</param>
    /// <returns>Returns the list of points around ship.</returns>
    IEnumerable<Point> ShipSafetyZone(Point[] ship);

    /// <summary>
    /// Checks if all ships are killed on map.
    /// </summary>
    /// <returns>True if all ships killed.</returns>
    bool IsAllShipsDestroyed(Map map);

    /// <summary>
    /// Implies damage to a ship.
    /// </summary>
    /// <param name="point">The point where damage is applied</param>
    void DamageShip(Map map, Point point);
}