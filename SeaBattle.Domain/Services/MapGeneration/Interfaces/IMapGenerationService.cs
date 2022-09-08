using SeaBattle.Domain.Exceptions;
using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Services.MapGeneration.Interfaces;

public interface IMapGenerationService
{
    /// <summary>
    /// Generates a map with ships based on values from <see cref="GameConfiguration"/>
    /// </summary>
    /// <returns>A map with ships</returns>
    /// <exception cref="GameException">Can be thrown when application is not able to place a ship after certain attempt</exception>
    Map GenerateRandomMapWithShips();
}