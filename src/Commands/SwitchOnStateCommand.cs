using CeloIsYou.Commands;

namespace CeloIsYou.src.Commands
{
    public class SwitchOnStateCommand : ICommand
    {
        public readonly Entity Entity;
        public readonly string Property;

        public SwitchOnStateCommand(Entity entity, string property)
        {
            Entity = entity;
            Property = property;
        }
    }
}
