using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Services.AI.Interfaces;

public interface IAiAnalyticsService
{
    /// <summary>
    /// Returns most probable places to fire.
    /// </summary>
    /// <returns>List of points where AI may fire.</returns>
    IEnumerable<Point> GetPossiblePlacesToFire(Map map, Point[] ship);
}