using CeloIsYou.Enumerations;

namespace CeloIsYou.Commands
{
    public class ChangeTypeCommand : ICommand
    {
        public readonly Entity Entity;
        public readonly EntityTypes FromType;
        public readonly EntityTypes ToType;

        public ChangeTypeCommand(Entity entity, EntityTypes toType)
        {
            Entity = entity;
            FromType = entity.Type;
            ToType = toType;
        }
    }
}
