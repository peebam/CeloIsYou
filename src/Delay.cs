using CeloIsYou.Extensions;
using Microsoft.Xna.Framework;

namespace CeloIsYou
{
    public class Delay
    {
        private readonly double _delay;
        private readonly double _start;
        public bool IsReached { get; private set; }

        public Delay(double delay)
        {
            _delay = delay;
            _start = 0;
        }

        public Delay(GameTime gameTime, double delay)
        {
            _delay = delay;
            _start = gameTime.ToTotalGameTimeSeconds();
        }

        public bool Update(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ToTotalGameTimeSeconds() - _start;
            IsReached = elapsedSeconds >= _delay;
            return IsReached;
        }
    }
}
