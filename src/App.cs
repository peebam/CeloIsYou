using System;
using System.Collections.Generic;
using System.Linq;
using CeloIsYou.Commands;
using CeloIsYou.Enumerations;
using CeloIsYou.Extensions;
using CeloIsYou.Handlers;
using CeloIsYou.Rules;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CeloIsYou
{
    public class App : Game
    {
        private readonly Grid _grid;
        private readonly Pipeline _pipeline;
        private readonly RenderTarget2D _renderTarget;
        private readonly Resources _resources;
        private readonly Rectangle _screen;
        private readonly SpriteBatch _spriteBatch;
        private readonly Stack<List<ICommand>> _turns;

        private readonly ICommandsHandler _doCommandsHandler;
        private readonly ICommandsHandler _unDoCommandsHandler;

        private Delay _inputDelayer;

        public App()
        {
            Content.RootDirectory = "Content";

            var graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = (int)(Configuration.Instance.CellWidth * Configuration.Instance.GridWidth * Configuration.Instance.RenderFactor),
                PreferredBackBufferHeight = (int)(Configuration.Instance.CellHeight * Configuration.Instance.GridHeight * Configuration.Instance.RenderFactor),
            };

            graphics.ApplyChanges();

            _grid = new Grid(Configuration.Instance.GridWidth, Configuration.Instance.GridHeight);
            _inputDelayer = new Delay(Configuration.Instance.GameSpeed);

            _pipeline = new Pipeline();
            _renderTarget = new RenderTarget2D(GraphicsDevice, Configuration.Instance.CellWidth * Configuration.Instance.GridWidth, Configuration.Instance.CellHeight * Configuration.Instance.GridHeight);
            _resources = new Resources(Content);
            _screen = new Rectangle(0, 0, (int)(Configuration.Instance.CellWidth * Configuration.Instance.GridWidth * Configuration.Instance.RenderFactor), (int)(Configuration.Instance.CellHeight * Configuration.Instance.GridHeight * Configuration.Instance.RenderFactor));
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _turns = new Stack<List<ICommand>>();

            _doCommandsHandler = new DoCommandsHandler(_pipeline, _resources, _grid);
            _unDoCommandsHandler = new UnDoCommandsHandler(_pipeline, _resources, _grid);
        }

        private void InitializeEntity(Entity entity, Coordinates to)
        {
            _grid.Enter(entity, to);
            entity.SetPosition(to.ToPosition());
            entity.SetTexture(_resources.GetEntityTexture(entity.Type));
            _pipeline.Subscribe(entity);
        }

        private void Move(Direction direction, GameTime gameTime)
        {
            var commands = new List<ICommand>();
            var controlledEntities = _grid.GetEntities().Where(e => e.IsControlled).ToList();
            while (controlledEntities.Any())
            {
                var entity = controlledEntities.ElementAt(0);
                MoveControlledEntity(entity, direction, gameTime, controlledEntities, commands);
            }

            UpdateRules(gameTime, commands);

            if (commands.Any())
                _turns.Push(commands);
        }

        private bool MoveControlledEntity(Entity entity, Direction direction, GameTime gameTime, List<Entity> controlledEntities, List<ICommand> commands)
        {
            if (!controlledEntities.Contains(entity))
                return false;

            controlledEntities.Remove(entity);
            return MoveEntity(entity, direction, gameTime, controlledEntities, commands);
        }

        private bool MoveEntity(Entity entity, Direction direction, GameTime gameTime, List<Entity> controlledEntities, List<ICommand> commands)
        {
            var currentCoordinates = _grid.GetCoordinates(entity);
            if (currentCoordinates == null)
                return false;

            if (!currentCoordinates.TryAdd(direction, out var nextCellCoordinates))
                return false;

            if (!_grid.IsMoveAllowed(entity, nextCellCoordinates))
                return false;

            var controlledEntitesCell = _grid.GetEntities(nextCellCoordinates, e => e.IsControlled);
            foreach (var controlledEntity in controlledEntitesCell)
                MoveControlledEntity(controlledEntity, direction, gameTime, controlledEntities, commands);

            var pushableEntitiesCell = _grid.GetEntities(nextCellCoordinates, e => e.IsPushable);
            foreach (var pushableEntity in pushableEntitiesCell)
                MoveEntity(pushableEntity, direction, gameTime, controlledEntities, commands);

            if (_grid.GetEntities(nextCellCoordinates, e => e.IsStoping).Any())
                return false;

            var command = new MoveInGridCommand(entity, currentCoordinates, nextCellCoordinates);
            commands.Add(command);
            _doCommandsHandler.Apply(command, gameTime);
            return true;
        }

        private void UnDoLastTurn(GameTime gameTime)
        {
            if (!_turns.Any())
                return;

            var commands = _turns.Pop();
            foreach (var command in commands)
                _unDoCommandsHandler.Apply(command, gameTime);

            UpdateRules(null, null);
        }

        private void UpdateRules(GameTime gameTime, List<ICommand> commands)
        {
            var entities = _grid.GetEntities();
            foreach (var entity in entities)
                entity.ClearStates();

            var expressions = _grid.GetExpressions();
            var parser = new Parser();
            parser.Process(expressions, entities);

            if (gameTime != null)
            {
                foreach (var command in parser.Commands)
                {
                    commands.Add(command);
                    _doCommandsHandler.Apply(command, gameTime);
                }
            }

            foreach (var rule in parser.Rules)
                rule.Apply(entities);
        }

        protected override void LoadContent()
        {
            _resources.Load();

            InitializeEntity(new Entity(EntityTypes.NatureText), new Coordinates(10, 0));
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(11, 0));
            InitializeEntity(new Entity(EntityTypes.ActionPush), new Coordinates(12, 0));
            InitializeEntity(new Entity(EntityTypes.ActionStop), new Coordinates(13, 5));

            InitializeEntity(new Entity(EntityTypes.NatureWall), new Coordinates(4, 1));
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(6, 1));
            InitializeEntity(new Entity(EntityTypes.NatureRock), new Coordinates(7, 1));
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(9, 1));
            InitializeEntity(new Entity(EntityTypes.NatureWall), new Coordinates(10, 1));

            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(8, 2));
            InitializeEntity(new Entity(EntityTypes.ActionPush), new Coordinates(8, 3));

            InitializeEntity(new Entity(EntityTypes.NatureCelo), new Coordinates(20, 2));
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(21, 2));
            InitializeEntity(new Entity(EntityTypes.ActionYou), new Coordinates(22, 2));

            InitializeEntity(new Entity(EntityTypes.ObjectSpot), new Coordinates(7, 7));
            InitializeEntity(new Entity(EntityTypes.ObjectSpot), new Coordinates(8, 8));
            InitializeEntity(new Entity(EntityTypes.ObjectSpot), new Coordinates(1, 1));
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 5));
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 6));
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 7));
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 8));
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 9));
            InitializeEntity(new Entity(EntityTypes.ObjectBox), new Coordinates(13, 12));
            InitializeEntity(new Entity(EntityTypes.ObjectBox), new Coordinates(14, 13));
            InitializeEntity(new Entity(EntityTypes.ObjectBox), new Coordinates(14, 14));

            InitializeEntity(new Entity(EntityTypes.ObjectCelo), new Coordinates(0, 0));

            UpdateRules(null, null);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_inputDelayer.Update(gameTime))
                return;

            if (Keyboard.GetState().GetPressedKeyCount() == 0)
                Configuration.Instance.GameSpeed = 0.15;
            else
                _inputDelayer = new Delay(gameTime, Configuration.Instance.GameSpeed);

            Direction direction = null;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                direction = Direction.Up;
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                direction = Direction.Down;
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                direction = Direction.Left;
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                direction = Direction.Right;
            else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Configuration.Instance.GameSpeed = Math.Max(Configuration.Instance.GameSpeed - 0.01, 0.02);
                UnDoLastTurn(gameTime);
            }

            if (direction != null)
                Move(direction, gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            _pipeline.Update(gameTime);

            _spriteBatch.Begin();

     
            var drawables = _pipeline.Drawables.OrderBy(d => d.DrawOrder);
            foreach (var drawable in drawables)
                _spriteBatch.Draw(drawable.Texture, drawable.Position, Color.White);
            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, _screen, Color.White);
            _spriteBatch.End();
        }
    }
}
