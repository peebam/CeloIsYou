using Microsoft.Xna.Framework.Graphics;

namespace CeloIsYou.Core
{
    public interface IAnimation : ISprite, IUpdatable
    {
        bool IsDone { get; }
    }
}
