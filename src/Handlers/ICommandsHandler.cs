using CeloIsYou.Commands;
using Microsoft.Xna.Framework;

namespace CeloIsYou.Handlers
{
    public interface ICommandsHandler
    {
        void Apply(ICommand command, GameTime gameTime);
    }
}
