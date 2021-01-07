using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou
{
    public class Pipeline
    {
        public IEnumerable<CeloIsYou.Core.IDrawable> Drawables => _drawables;
        private readonly List<CeloIsYou.Core.IDrawable> _drawables;

        public Pipeline()
        {
            _drawables = new List<CeloIsYou.Core.IDrawable>();
        }

        public void Subscribe(CeloIsYou.Core.IDrawable drawable)
        {
            if (drawable == null)
                throw new ArgumentNullException(nameof(drawable));

            _drawables.Add(drawable);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var drawable in _drawables)
                drawable.Update(gameTime);
        }

        public void Unsubscribe(CeloIsYou.Core.IDrawable drawable)
        {
            if (drawable == null)
                throw new ArgumentNullException(nameof(drawable));

            _drawables.Remove(drawable);
        }
    }
}
