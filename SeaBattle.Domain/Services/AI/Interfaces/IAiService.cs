using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Services.AI.Interfaces;

public interface IAiService
{
    /// <summary>
    /// Calculates possible place for computer to fire.
    /// </summary>
    /// <returns>Point where computer suggests to fire.</returns>
    Point GetPossibleMove(Map map);
}