using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;

namespace SeaBattle.Domain.Services;

public class CannonService : ICannonService
{
    private readonly IShipService _shipService;

    public CannonService(IShipService shipService)
    {
        _shipService = shipService;
    }

    public void Shoot(Map map, Point point)
    {
        var fieldType = map.GetField(point);
        
        switch (fieldType)
        {
            case FieldType.See:
                HandleSeeDamage(map, point);
                break;
            case FieldType.Boat:
                HandleShipDamage(map, point);
                break;
        }
    }

    private void HandleSeeDamage(Map map, Point point)
    {
        map.SetField(point, FieldType.CheckedSee);
    }

    private void HandleShipDamage(Map map, Point point)
    {
        _shipService.DamageShip(map, point);
    }
}