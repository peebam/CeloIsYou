


namespace CeloIsYou.Commands
{
    public class ExitGameCommand : ICommand
    {
        public readonly Coordinates Coordinates;
        public readonly Entity Entity;

        public ExitGameCommand(Entity entity)
        {
            Entity = entity;
            Coordinates = entity.Coordinates;
        }
    }
}
