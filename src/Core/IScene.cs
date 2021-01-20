
using Microsoft.Xna.Framework;
using System;

namespace CeloIsYou.Core
{
    public interface IScene : IDisposable
    {
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime); 
    }
}
