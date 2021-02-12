using CeloIsYou.Extensions;
using Microsoft.Xna.Framework;

namespace CeloIsYou
{
    public class Timeout
    {
        private readonly double _delay;
        private readonly double _start;
        public bool IsReached { get; private set; }

        public Timeout(double delay)
        {
            _delay = delay;
            _start = 0;
        }

        public Timeout(GameTime gameTime, double delay)
        {
            _delay = delay;
            _start = gameTime.ToTotalGameTimeSeconds();
        }

        public bool IsDone(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ToTotalGameTimeSeconds() - _start;
            IsReached = elapsedSeconds >= _delay;
            return IsReached;
        }
    }
}
