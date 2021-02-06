namespace CeloIsYou.Commands
{
    public class MoveInGridCommand : ICommand
    {
        public readonly Entity Entity;
        public readonly Coordinates FromCoordinates;
        public readonly Coordinates ToCoordinates;

        public MoveInGridCommand(Entity entity,  Coordinates toCoordinates)
        {
            Entity = entity;
            FromCoordinates = entity.Coordinates;
            ToCoordinates = toCoordinates;
        }
    }
}
