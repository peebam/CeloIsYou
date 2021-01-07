using Microsoft.Xna.Framework;

namespace CeloIsYou.Extensions
{
    public static class GameTimeExtension
    {
        public static double ToTotalGameTimeSeconds(this GameTime gameTime)
            => gameTime.TotalGameTime.TotalSeconds;
    }
}
