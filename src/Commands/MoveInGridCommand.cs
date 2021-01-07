namespace CeloIsYou.Commands
{
    public class MoveInGridCommand : ICommand
    {
        public readonly Entity Entity;
        public readonly Coordinates FromCoordinates;
        public readonly Coordinates ToCoordinates;

        public MoveInGridCommand(Entity entity, Coordinates fromCoordinates, Coordinates toCoordinates)
        {
            Entity = entity;
            FromCoordinates = fromCoordinates;
            ToCoordinates = toCoordinates;
        }
    }
}
