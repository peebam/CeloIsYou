
using CeloIsYou.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace CeloIsYou
{
    public class Sprite : IEntity
    {
        public int DrawOrder => 0;

        public Vector2 Position { get; }

        public bool Visible => true;

        public Texture2D Texture => _animation.Texture;

        public bool IsDone => _animation.IsDone;


        private IAnimation _animation;
        
        public Sprite(Vector2 position, IAnimation animation)
        {
            Position = position;
            _animation = animation;
        }

        public void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }
    }
}
