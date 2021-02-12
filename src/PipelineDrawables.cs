using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou
{
    public class PipelineDrawables
    {
        private readonly List<Core.IDrawable> _drawables;

        private readonly SpriteBatch _spriteBatch;

        public PipelineDrawables(SpriteBatch spriteBatch)
        {
            _drawables = new List<Core.IDrawable>();
            _spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
            var drawables = _drawables.Where(d => d.Visible)
                                      .OrderBy(d => d.DrawOrder);

            foreach (var drawable in drawables)
                _spriteBatch.Draw(drawable.Texture, drawable.Position, Color.White);
        }

        public void Subscribe(Core.IDrawable drawable)
            => _drawables.Add(drawable);
        
        public void Unsubscribe(Core.IDrawable drawable)
            => _drawables.Remove(drawable);
    }
}
