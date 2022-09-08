using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Interfaces;

public interface ICannonService
{
    /// <summary>
    /// Produce a cannon fire.
    /// </summary>
    /// <param name="map">The player map.</param>
    /// <param name="point">Point where to fire.</param>
    void Shoot(Map map, Point point);
}