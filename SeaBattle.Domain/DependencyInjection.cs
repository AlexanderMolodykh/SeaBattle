using Microsoft.Extensions.DependencyInjection;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Domain.Services;
using SeaBattle.Domain.Services.AI;
using SeaBattle.Domain.Services.AI.Interfaces;
using SeaBattle.Domain.Services.MapGeneration;
using SeaBattle.Domain.Services.MapGeneration.Interfaces;

namespace SeaBattle.Domain
{
    public static class DependencyInjection
    {
        public static void RegisterDomain(this ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMapGenerationService, MapGenerationService>();
            serviceCollection.AddTransient<IShipService, ShipService>();
            serviceCollection.AddTransient<IShipPlacementService, ShipPlacementService>();
            serviceCollection.AddTransient<IAiService, AiService>();
            serviceCollection.AddTransient<IAiAnalyticsService, AiAnalyticsService>();
            serviceCollection.AddTransient<ICannonService, CannonService>();
            serviceCollection.AddSingleton<IRandomService, RandomService>();
        }
    }
}
