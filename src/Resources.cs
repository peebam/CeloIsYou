using System;
using System.Collections.Generic;
using CeloIsYou.Animation;
using CeloIsYou.Core;
using CeloIsYou.Enumerations;
using CeloIsYou.Extensions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace CeloIsYou
{
    public class Resources
    {
        private readonly ContentManager _contentManager;
        private readonly Dictionary<string, Texture2D> _pictures;

        public Resources(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _pictures = new Dictionary<string, Texture2D>();
        }

        public IAnimation GetAnimation(string name)
            => new OneFrameAnimation(_pictures[name]);

        public IAnimation GetAnimationSmoke(float frameLast)
        {
            return new BasicAnimation(new[] {
                _pictures["Others/Smoke/Smoke_01"],
                _pictures["Others/Smoke/Smoke_02"],
                _pictures["Others/Smoke/Smoke_03"],
                _pictures["Others/Smoke/Smoke_04"]
            }, frameLast);
        }
        
        public void Load()
        {
            foreach (var type in (EntityTypes[])Enum.GetValues(typeof(EntityTypes)))
            {
               
                var contentName = type.ToContentName();
                var picture = _contentManager.Load<Texture2D>(contentName);
                _pictures[contentName] = picture;
            }

            _pictures["Others/Smoke/Smoke_01"] = _contentManager.Load<Texture2D>("Others/Smoke/Smoke_01");
            _pictures["Others/Smoke/Smoke_02"] = _contentManager.Load<Texture2D>("Others/Smoke/Smoke_02");
            _pictures["Others/Smoke/Smoke_03"] = _contentManager.Load<Texture2D>("Others/Smoke/Smoke_03");
            _pictures["Others/Smoke/Smoke_04"] = _contentManager.Load<Texture2D>("Others/Smoke/Smoke_04");
        }
    }
}
