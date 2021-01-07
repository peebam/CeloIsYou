using System;
using CeloIsYou.Commands;
using CeloIsYou.Extensions;
using Microsoft.Xna.Framework;

namespace CeloIsYou.Handlers
{
    public class DoCommandsHandler : ICommandsHandler
    {
        private Grid _grid;
        private Pipeline _pipeline;
        private Resources _resources;

        public DoCommandsHandler(Pipeline pipeline, Resources resources, Grid grid)
        {
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public void Apply(ICommand command, GameTime gameTime)
        {
            var typeOf = command.GetType().Name;
            switch (typeOf)
            {
                case nameof(ChangeTypeCommand): Apply((ChangeTypeCommand)command, gameTime); return;
                case nameof(EnterGameCommand): Apply((EnterGameCommand)command, gameTime); return;
                case nameof(ExitGameCommand): Apply((ExitGameCommand)command, gameTime); return;
                case nameof(MoveInGridCommand): Apply((MoveInGridCommand)command, gameTime); return;
                default: throw new ArgumentOutOfRangeException(nameof(command));
            }
        }

        private void Apply(ChangeTypeCommand command, GameTime gameTime)
        {
            command.Entity.Type = command.ToType;
            command.Entity.SetTexture(_resources.GetEntityTexture(command.Entity.Type));
        }

        private void Apply(EnterGameCommand command, GameTime gameTime)
        {
            _grid.Enter(command.Entity, command.Coordinates);
            command.Entity.SetPosition(command.Coordinates.ToPosition(), gameTime);
            command.Entity.SetTexture(_resources.GetEntityTexture(command.Entity.Type));
            _pipeline.Subscribe(command.Entity);
        }

        private void Apply(ExitGameCommand command, GameTime gameTime)
        {
            _grid.Exit(command.Entity);
            command.Entity.ClearPosition();
            _pipeline.Unsubscribe(command.Entity);
        }

        private void Apply(MoveInGridCommand command, GameTime gameTime)
        {
            _grid.Move(command.Entity, command.ToCoordinates);
            command.Entity.SetPosition(command.ToCoordinates.ToPosition(), gameTime);
        }
    }
}
