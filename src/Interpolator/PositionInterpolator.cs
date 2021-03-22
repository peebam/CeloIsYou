using System;
using Microsoft.Xna.Framework;

namespace CeloIsYou.Interpolator
{
    public class PositionInterpolator
    {
        private readonly double _animationLast;
        private readonly Vector2 _fromValue;

        private readonly double _initialGameTime;
        private readonly Vector2 _toValue;
        private readonly Action<PositionInterpolator> _update;
        public Vector2 Current { get; private set; }
        public bool IsDone { get; private set; }

        public PositionInterpolator(Vector2 fromValue, Vector2 toValue, double animationLast, GameTime gameTime, Action<PositionInterpolator> update)
        {
            _animationLast = animationLast;
            _fromValue = fromValue;
            _initialGameTime = gameTime?.TotalGameTime.TotalSeconds ?? throw new ArgumentNullException(nameof(gameTime));
            _toValue = toValue;
            _update = update;
        }

        public void Update(GameTime currentGameTime)
        {
            if (IsDone)
                return;
            
            var elapsedTime = currentGameTime.TotalGameTime.TotalSeconds - _initialGameTime;
            var interpolationPosition = (float)Math.Clamp(elapsedTime / _animationLast, 0d, 1d);

            Current = Vector2.Lerp(_fromValue, _toValue, interpolationPosition);

            _update(this);

            IsDone = interpolationPosition == 1d;
        }
    }
}
