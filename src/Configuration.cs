namespace CeloIsYou
{
    public class Configuration
    {
        public readonly int CellHeight = 24;
        public readonly int CellWidth = 24;
        public readonly int GridHeight = 16;
        public readonly int GridWidth = 26;
        
        public double GameSpeed { get; set; }
        public double RenderFactor { get; set; }

        public readonly static Configuration Instance = new Configuration();

        public Configuration()
        {
            GameSpeed = 0.15;
            RenderFactor = 2;
        }
    }
}
