using CeloIsYou.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou.Animation
{
    class OneFrameAnimation : IAnimation
    {
        public bool IsDone => false;

        public Texture2D Texture { get; }

        public OneFrameAnimation(Texture2D texture)
        {
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}
