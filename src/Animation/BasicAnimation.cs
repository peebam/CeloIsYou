using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CeloIsYou.Core
{
    public class BasicAnimation : IAnimation
    {
        private int _currentFrame;
        private IList<Texture2D> _frames;
        private double _frameLast;
        private double _unconsumedTime;
       
        public bool IsDone { get; }
        public Texture2D Texture => _frames[_currentFrame];

        public BasicAnimation(IList<Texture2D> frames, double frameLast)
        {
            _unconsumedTime = 0;
            _currentFrame = 0;
            _frameLast = frameLast;
            _frames = frames ?? throw new ArgumentNullException(nameof(frames));
        }

        public void Update(GameTime gameTime)
        {
            var elapsedTime = _unconsumedTime + gameTime.ElapsedGameTime.TotalSeconds;
            var frameAdvance = (int)Math.Floor(elapsedTime / _frameLast);

            _currentFrame = (_currentFrame + frameAdvance) % _frames.Count;
            _unconsumedTime = elapsedTime % _frameLast;
        }
    }
}
