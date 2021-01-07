


namespace CeloIsYou.Commands
{
    public class ExitGameCommand : ICommand
    {
        public readonly Coordinates Coordinates;
        public readonly Entity Entity;

        public ExitGameCommand(Entity entity, Coordinates coordinates)
        {
            Entity = entity;
            Coordinates = coordinates;
        }
    }
}
