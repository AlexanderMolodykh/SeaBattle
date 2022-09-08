namespace SeaBattle.Domain.Configuration
{
    public class GameConfiguration
    {
        public GameConfiguration()
        {
            MapSize = 10;
            MapGeneratorMaxNumberOfRetries = 100;
            ShipTypes = new int[] { 1, 1, 1, 1, 2, 2, 2, 3, 3, 4 };
        }

        public int[] ShipTypes { get; set; }
        public int MapSize { get; set; }
        public int MapGeneratorMaxNumberOfRetries { get; set; }
    }
}