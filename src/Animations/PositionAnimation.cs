using System;
using Microsoft.Xna.Framework;

namespace CeloIsYou.Animations
{
    public class PositionAnimation : IAnimation
    {
        private readonly double _animationLast;
        private readonly Vector2 _fromPosition;

        private readonly double _initialGameTime;
        private readonly Vector2 _toPosition;
        private readonly Action<PositionAnimation> _update;
        public Vector2 CurrentPosition { get; private set; }
        public bool IsDone { get; private set; }

        public PositionAnimation(Vector2 fromPosition, Vector2 toPosition, double animationLast, GameTime gameTime, Action<PositionAnimation> update)
        {
            _animationLast = animationLast;
            _fromPosition = fromPosition;
            _initialGameTime = gameTime?.TotalGameTime.TotalSeconds ?? throw new ArgumentNullException(nameof(gameTime));
            _toPosition = toPosition;
            _update = update;
        }

        public void Update(GameTime currentGameTime)
        {
            if (IsDone)
                return;

            var elapsedTime = currentGameTime.TotalGameTime.TotalSeconds - _initialGameTime;
            var interpolationPosition = (float)Math.Clamp(elapsedTime / _animationLast, 0d, 1d);
            CurrentPosition = Vector2.Lerp(_fromPosition, _toPosition, interpolationPosition);

            _update(this);

            IsDone = interpolationPosition == 1d;
        }
    }
}