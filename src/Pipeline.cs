using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou
{
    public class Pipeline
    {
        public IEnumerable<Core.IDrawable> Drawables => _drawables;
        private readonly List<Core.IDrawable> _drawables;

        private readonly SpriteBatch _spriteBatch;

        public Pipeline(SpriteBatch spriteBatch)
        {
            _drawables = new List<Core.IDrawable>();
            _spriteBatch = spriteBatch;
        }

        private void Update(GameTime gameTime)
        {
            foreach (var drawable in _drawables)
                drawable.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Update(gameTime);
            var drawables = Drawables.OrderBy(d => d.DrawOrder);
            foreach (var drawable in drawables)
                _spriteBatch.Draw(drawable.Texture, drawable.Position, Color.White);
        }

        public void Subscribe(Core.IDrawable drawable)
        {
            if (drawable == null)
                throw new ArgumentNullException(nameof(drawable));

            _drawables.Add(drawable);
        }

        public void Unsubscribe(Core.IDrawable drawable)
        {
            if (drawable == null)
                throw new ArgumentNullException(nameof(drawable));

            _drawables.Remove(drawable);
        }
    }
}
