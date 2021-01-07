using Microsoft.Xna.Framework;

namespace CeloIsYou.Animations
{
    public interface IAnimation
    {
        bool IsDone { get; }
        void Update(GameTime currentGameTime);
    }
}
