using Microsoft.Extensions.Options;
using SeaBattle.Domain.Configuration;
using SeaBattle.Domain.Exceptions;
using SeaBattle.Domain.Models;
using SeaBattle.Domain.Services.MapGeneration.Interfaces;

namespace SeaBattle.Domain.Services.MapGeneration
{
    public class MapGenerationService : IMapGenerationService
    {
        private readonly IShipPlacementService _shipPlacementService;
        private readonly GameConfiguration _gameConfiguration;


        public MapGenerationService(IShipPlacementService shipPlacementService, IOptions<GameConfiguration> domainConfiguration)
        {
            _shipPlacementService = shipPlacementService;
            _gameConfiguration = domainConfiguration.Value;
        }

        public Map GenerateRandomMapWithShips()
        {
            var map = new Map(_gameConfiguration.MapSize);

            foreach (var shipType in _gameConfiguration.ShipTypes.OrderByDescending(size => size))
            {
                Point[]? ship = null;
                for (var i = 0; i < _gameConfiguration.MapGeneratorMaxNumberOfRetries; i++)
                {
                    var shipPosition = _shipPlacementService.FindRandomPosition(map.Size, shipType);
                    var isValidShipPosition = _shipPlacementService.ValidateShipPosition(map, shipPosition);

                    if (isValidShipPosition)
                    {
                        ship = shipPosition;
                        break;
                    }
                }

                if (ship == null)
                {
                    throw new GameException("Not able to place a ship");
                }

                map.AddShip(ship);
            }

            return map;
        }
    }
}