using Microsoft.Xna.Framework;

namespace CeloIsYou.Extensions
{
    public static class CoordinatesExtension
    {
        public static Vector2 ToPosition(this Coordinates coordinates)
            => new(coordinates.X * Configuration.Instance.CellWidth, coordinates.Y * Configuration.Instance.CellHeight);
    }
}
