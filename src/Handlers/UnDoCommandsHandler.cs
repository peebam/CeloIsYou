
using System;
using CeloIsYou.Commands;
using CeloIsYou.Extensions;
using Microsoft.Xna.Framework;

namespace CeloIsYou.Handlers
{
    public class UnDoCommandsHandler : ICommandsHandler
    {
        private Grid _grid;
        private Pipeline _pipeline;
        private Resources _resources;

        public UnDoCommandsHandler(Pipeline pipeline, Resources resources, Grid grid)
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
            command.Entity.Type = command.FromType;
            command.Entity.SetTexture(_resources.GetEntityTexture(command.Entity.Type));
        }

        private void Apply(EnterGameCommand command, GameTime gameTime)
        {
            _grid.Exit(command.Entity);
            command.Entity.ClearPosition();
            _pipeline.Unsubscribe(command.Entity);
        }

        private void Apply(ExitGameCommand command, GameTime gameTime)
        {
            _grid.Enter(command.Entity, command.Coordinates);
            command.Entity.SetPosition(command.Coordinates.ToPosition(), gameTime);
            _pipeline.Subscribe(command.Entity);
        }

        private void Apply(MoveInGridCommand command, GameTime gameTime)
        {
            _grid.Move(command.Entity, command.FromCoordinates);
            command.Entity.SetPosition(command.FromCoordinates.ToPosition(), gameTime);
        }
    }
}
