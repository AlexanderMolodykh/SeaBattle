using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.AI.Interfaces;
using SeaBattle.Domain.Services.MapGeneration.Interfaces;

namespace SeaBattle.ViewModels;

public class PlayerViewModel : BindableBase
{
    private readonly IMapGenerationService _mapGenerationService;
    private readonly ICannonService _cannonService;
    private readonly IAiService _aiService;
    private readonly IShipService _shipService;
    private bool _isWinner;
    private List<List<BattleCellViewModel>> _map;
    private Map _playerMap;

    public PlayerViewModel(IMapGenerationService mapGenerationService, ICannonService cannonService, IAiService aiService, IShipService shipService)
    {
        _mapGenerationService = mapGenerationService;
        _cannonService = cannonService;
        _aiService = aiService;
        _shipService = shipService;
    }
        
    public void Start()
    {
        _playerMap = _mapGenerationService.GenerateRandomMapWithShips();
        Map = MapMapping(_playerMap);
        isWinner = false;
    }

    public void Fire(Point? point)
    {
        point ??= _aiService.GetPossibleMove(_playerMap);

        _cannonService.Shoot(_playerMap, point.Value);

        Map = MapMapping(_playerMap);

        if (_shipService.IsAllShipsDestroyed(_playerMap))
        {
            isWinner = true;
        }
    }

    public bool isWinner
    {
        get => _isWinner;
        set => base.SetProperty(ref _isWinner, value);
    }

    public List<List<BattleCellViewModel>> Map
    {
        get => _map;
        set => base.SetProperty(ref _map, value);
    }

    private List<List<BattleCellViewModel>> MapMapping(Map map)
    {
        return map.EnumerateFields()
            .GroupBy(point => point.point.x)
            .Select(groupByRow => groupByRow.Select(mapPoint =>
                new BattleCellViewModel
                {
                    Type = mapPoint.field,
                    Point = mapPoint.point
                }).ToList()).ToList();
    }
}