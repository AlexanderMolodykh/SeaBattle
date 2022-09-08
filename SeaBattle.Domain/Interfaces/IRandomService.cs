namespace SeaBattle.Domain.Interfaces;

public interface IRandomService
{
    int Next(int min, int max);
}