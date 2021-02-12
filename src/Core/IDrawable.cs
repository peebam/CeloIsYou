using Microsoft.Xna.Framework;

namespace CeloIsYou.Core
{
    public interface IDrawable : ISprite
    {
        int DrawOrder { get; }
        Vector2 Position { get; }
        bool Visible { get; }
    }
}
