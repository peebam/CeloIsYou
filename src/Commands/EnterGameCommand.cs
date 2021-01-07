namespace CeloIsYou.Commands
{
    public class EnterGameCommand : ICommand
    {
        public readonly Coordinates Coordinates;
        public readonly Entity Entity;

        public EnterGameCommand(Entity entity, Coordinates coordinates)
        {
            Entity = entity;
            Coordinates = coordinates;
        }
    }
}
