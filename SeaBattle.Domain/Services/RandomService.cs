using SeaBattle.Domain.Interfaces;

namespace SeaBattle.Domain.Services;

public class RandomService : IRandomService
{
    private readonly Random _random;

    public RandomService()
    {
        _random = new Random();
    }

    public int Next(int min, int max)
    {
        return _random.Next(min, max);
    }
}