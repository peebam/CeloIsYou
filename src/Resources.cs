


using System;
using System.Collections.Generic;
using CeloIsYou.Enumerations;
using CeloIsYou.Extensions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace CeloIsYou
{
    public class Resources
    {
        private readonly ContentManager _contentManager;
        private readonly Dictionary<EntityTypes, Texture2D> _pictures;

        public Resources(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _pictures = new Dictionary<EntityTypes, Texture2D>();
        }

        public Texture2D GetEntityTexture(EntityTypes type)
            => _pictures[type];

        public void Load()
        {
            foreach (var type in (EntityTypes[])Enum.GetValues(typeof(EntityTypes)))
            {
                var contentName = type.ToContentName();
                var picture = _contentManager.Load<Texture2D>(contentName);
                _pictures[type] = picture;
            }
        }
    }
}
