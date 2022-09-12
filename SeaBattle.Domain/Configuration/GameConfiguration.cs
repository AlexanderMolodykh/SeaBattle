namespace SeaBattle.Domain.Configuration
{
    public class GameConfiguration
    {
        public int[] ShipTypes { get; set; }
        public int MapSize { get; set; }
        public int MapGeneratorMaxNumberOfRetries { get; set; }
        public int ComputerMoveDelay { get; set; }
    }
}