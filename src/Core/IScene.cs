
using Microsoft.Xna.Framework;
using System;

namespace CeloIsYou.Core
{
    public interface IScene : IDisposable, IUpdatable
    {
        void Draw(GameTime gameTime);
    }
}
