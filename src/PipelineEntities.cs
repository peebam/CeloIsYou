using System;
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

        private readonly PipelineDrawables _pipelineDrawables;

        public PipelineEntities(PipelineDrawables pipelineDrawables)
        {
            _entities = new List<IEntity>();
            _pipelineDrawables = pipelineDrawables;
        }

        public void Update(GameTime gameTime)
        {
            _entities.UpdateAll(gameTime);

            var doneEntities = _entities.Where(e => e.IsDone).ToList();
            foreach (var entity in doneEntities)
                Unsubscribe(entity);
        }

        public void Subscribe(IEntity entity)
        {
            _entities.Add(entity);
            _pipelineDrawables.Subscribe(entity);
        }

        public void Unsubscribe(IEntity entity)
        {
            _entities.Remove(entity);
            _pipelineDrawables.Unsubscribe(entity);
        }
    }
}
