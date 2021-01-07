using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou.Core
{
    public interface IDrawable
    {
        int DrawOrder { get; }
        Vector2 Position { get; }
        Texture2D Texture { get; }
        
        void Update(GameTime gameTime);
    }
}
