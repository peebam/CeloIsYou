using System.Collections.Generic;
using System.Linq;
using CeloIsYou.Core;
using CeloIsYou.src.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou
{
    public class PipelineEntities
    {
        private readonly List<IEntity> _entities;
        
        private readonly SpriteBatch _spriteBatch;

        public PipelineEntities(SpriteBatch spriteBatch)
        {
            _entities = new List<IEntity>();
            _spriteBatch = spriteBatch;
        }

        public void Draw(GameTime gameTime)
        {
            var drawables = _entities.Where(d => d.Visible)
                                     .OrderBy(d => d.DrawOrder);
            
            foreach (var drawable in drawables)
                _spriteBatch.Draw(drawable.Texture, drawable.Position, Color.White);
        }

        public void Subscribe(IEntity entity)
            => _entities.Add(entity);
        
        public void Unsubscribe(IEntity entity)
            => _entities.Remove(entity);
        
        public void Update(GameTime gameTime)
        {
            _entities.UpdateAll(gameTime);

            var doneEntities = _entities.Where(e => e.IsDone).ToList();
            foreach (var entity in doneEntities)
                Unsubscribe(entity);
        }
    }
}
