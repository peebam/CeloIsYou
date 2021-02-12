using CeloIsYou.Commands;
using CeloIsYou.Core;
using CeloIsYou.Enumerations;
using CeloIsYou.Extensions;
using CeloIsYou.Handlers;
using CeloIsYou.Rules;
using CeloIsYou.src.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CeloIsYou
{
    public class Level : IScene
    {
        private bool _initialized = false;

        private readonly GraphicsDevice _graphicsDevice;
        private readonly Grid _grid;
        private readonly Parser _parser;
        private readonly PipelineDrawables _pipelineDrawables;
        private readonly PipelineEntities _pipelineEntities;
        private readonly RenderTarget2D _renderTarget;
        private readonly Resources _resources;
        private readonly Rectangle _screen;
        private readonly SpriteBatch _spriteBatch;

        private Stack<ICommand> _turn;
        private readonly Stack<Stack<ICommand>> _turns;

        private readonly ICommandsHandler _doCommandsHandler;
        private readonly ICommandsHandler _unDoCommandsHandler;

        private Delay _inputDelayer;

        public Level(GraphicsDevice graphicsDevice, Resources resources)
        {

            _graphicsDevice = graphicsDevice;
            _grid = new Grid(Configuration.Instance.GridWidth, Configuration.Instance.GridHeight);
            _inputDelayer = new Delay(Configuration.Instance.GameSpeed);
            _parser = new Parser();
           
            _renderTarget = new RenderTarget2D(_graphicsDevice, Configuration.Instance.CellWidth * Configuration.Instance.GridWidth, Configuration.Instance.CellHeight * Configuration.Instance.GridHeight);
            _resources = resources;
            _screen = new Rectangle(0, 0, (int)(Configuration.Instance.CellWidth * Configuration.Instance.GridWidth * Configuration.Instance.RenderFactor), (int)(Configuration.Instance.CellHeight * Configuration.Instance.GridHeight * Configuration.Instance.RenderFactor));
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _turns = new Stack<Stack<ICommand>>();

            _pipelineDrawables = new PipelineDrawables(_spriteBatch);
            _pipelineEntities = new PipelineEntities(_pipelineDrawables);
            
            _doCommandsHandler = new DoCommandsHandler(_pipelineEntities, _resources, _grid);
            _unDoCommandsHandler = new UnDoCommandsHandler(_pipelineEntities, _resources, _grid);
        }

        private void Initialize(GameTime gameTime)
        {
            InitializeEntity(new Entity(EntityTypes.NatureText), new Coordinates(10, 0), gameTime);
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(11, 0), gameTime);
            InitializeEntity(new Entity(EntityTypes.StatePush), new Coordinates(12, 0), gameTime);
            InitializeEntity(new Entity(EntityTypes.StateStop), new Coordinates(13, 5), gameTime);

            InitializeEntity(new Entity(EntityTypes.NatureWall), new Coordinates(6, 1), gameTime);
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(7, 1), gameTime);
            InitializeEntity(new Entity(EntityTypes.NatureRock), new Coordinates(8, 1), gameTime);
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(9, 1), gameTime);
            InitializeEntity(new Entity(EntityTypes.NatureWall), new Coordinates(10, 1), gameTime);

            InitializeEntity(new Entity(EntityTypes.NatureCelo), new Coordinates(5, 3), gameTime);

            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(8, 2), gameTime);
            InitializeEntity(new Entity(EntityTypes.StatePush), new Coordinates(8, 3), gameTime);

            InitializeEntity(new Entity(EntityTypes.NatureCelo), new Coordinates(20, 2), gameTime);
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(21, 2), gameTime);
            InitializeEntity(new Entity(EntityTypes.ActionYou), new Coordinates(22, 2), gameTime);

            InitializeEntity(new Entity(EntityTypes.NatureCelo), new Coordinates(20, 3), gameTime);
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(21, 3), gameTime);
            InitializeEntity(new Entity(EntityTypes.StateWeak), new Coordinates(22, 3), gameTime);


            InitializeEntity(new Entity(EntityTypes.NatureWall), new Coordinates(1, 12), gameTime);
            InitializeEntity(new Entity(EntityTypes.VerbIs), new Coordinates(2, 12), gameTime);
            InitializeEntity(new Entity(EntityTypes.StateKill), new Coordinates(3, 12), gameTime);

            InitializeEntity(new Entity(EntityTypes.ObjectSpot), new Coordinates(7, 7), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectSpot), new Coordinates(8, 8), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectSpot), new Coordinates(1, 1), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 5), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 6), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 7), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 8), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectWall), new Coordinates(5, 9), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectBox), new Coordinates(13, 12), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectBox), new Coordinates(14, 13), gameTime);
            InitializeEntity(new Entity(EntityTypes.ObjectBox), new Coordinates(14, 14), gameTime);

            InitializeEntity(new Entity(EntityTypes.ObjectCelo), new Coordinates(0, 0), gameTime);

            UpdateRules(gameTime);
        }

        private void InitializeEntity(Entity entity, Coordinates to, GameTime gameTime)
        {
            var command = new EnterGameCommand(entity, to);
            _doCommandsHandler.Apply(command, gameTime);
        }

        private void Move(Direction direction, GameTime gameTime)
        {
            if (_turn != null)
                return;

            _turn = new Stack<ICommand>();

            var controlledEntities = _grid.GetEntities().Where(e => e.IsControlled).ToList();
            while (controlledEntities.Any())
            {
                var entity = controlledEntities.ElementAt(0);
                MoveControlledEntity(entity, direction, gameTime, controlledEntities);
            }
        }

        private bool MoveControlledEntity(Entity entity, Direction direction, GameTime gameTime, List<Entity> controlledEntities)
        {
            if (!controlledEntities.Contains(entity))
                return false;

            _pipelineEntities.Subscribe(new Sprite(entity.Position, _resources.GetAnimationSmoke()));

            controlledEntities.Remove(entity);
            return MoveEntity(entity, direction, gameTime, controlledEntities);
        }

        private bool MoveEntity(Entity entity, Direction direction, GameTime gameTime, List<Entity> controlledEntities)
        {
            if (entity.Coordinates == null)
                return false;

            if (!entity.Coordinates.TryAdd(direction, out var nextCellCoordinates))
                return false;

            if (!_grid.IsMoveAllowed(entity, nextCellCoordinates))
                return false;

            var controlledEntitesCell = _grid.GetEntities(nextCellCoordinates, e => e.IsControlled);
            foreach (var controlledEntity in controlledEntitesCell)
                MoveControlledEntity(controlledEntity, direction, gameTime, controlledEntities);

            var pushableEntitiesCell = _grid.GetEntities(nextCellCoordinates, e => e.IsPushable);
            foreach (var pushableEntity in pushableEntitiesCell)
                MoveEntity(pushableEntity, direction, gameTime, controlledEntities);

            if (_grid.GetEntities(nextCellCoordinates, e => e.IsStoping).Any())
                return false;

            var command = new MoveInGridCommand(entity, nextCellCoordinates);
            _turn.Push(command);
            _doCommandsHandler.Apply(command, gameTime);
            return true;
        }

        private void UnDoLastTurn(GameTime gameTime)
        {
            if (!_turns.Any())
                return;

            var turn = _turns.Pop();
            while(turn.Any())
            {
                var command = turn.Pop();
                _unDoCommandsHandler.Apply(command, gameTime);
            }

            UpdateRules(); 
        }

        private IReadOnlyCollection<ICommand> UpdateRules(GameTime gameTime)
        {
            var entities = _grid.GetEntities();
            var expressions = _grid.GetExpressions();
            var result = _parser.Process(expressions, entities);

            foreach (var command in result.Commands) 
                _doCommandsHandler.Apply(command, gameTime);
            
            entities = _grid.GetEntities();
            entities.ClearStatesAll();
            result.Rules.ApplyAll(entities);
            
            List<ICommand> commands = new List<ICommand>();
            var weakEntities = entities.Where(e => e.IsWeak);
            foreach(var weakEntity in weakEntities)
            {
                
                if (!_grid.HasEntities(weakEntity.Coordinates, e => e.IsKilling && e != weakEntity))
                    continue;

                var command = new ExitGameCommand(weakEntity);
                _doCommandsHandler.Apply(command, gameTime);
                commands.Add(command);
            }

            return result.Commands.Concat(commands).ToList();
        }

        private void UpdateRules()
        {
            var entities = _grid.GetEntities();
            var expressions = _grid.GetExpressions();
            var result = _parser.Process(expressions, entities);

            entities = _grid.GetEntities();
            foreach (var entity in entities)
                entity.ClearStates();

            foreach (var rule in result.Rules)
                rule.Apply(entities);
        }

        public void Draw(GameTime gameTime)
        {
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _pipelineDrawables.Draw(gameTime);
            _spriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, _screen, Color.White);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (!_initialized)
            {
                Initialize(gameTime);
                _initialized = true;
            }

            if (!_inputDelayer.Update(gameTime))
            {
                _pipelineEntities.Update(gameTime);
                return;
            }

            if (_turn != null)
            {
                var commands = UpdateRules(gameTime);
                _turn.PushAll(commands);

                if (_turn.Any())
                    _turns.Push(_turn);

                _turn = null;
            }

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
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Configuration.Instance.GameSpeed = Math.Max(Configuration.Instance.GameSpeed - 0.01, 0.02);
                UnDoLastTurn(gameTime);
            }

            if (direction != null)
                Move(direction, gameTime);

            _pipelineEntities.Update(gameTime);
        }

        public void Dispose()
        {
            _renderTarget.Dispose();
            _spriteBatch.Dispose();
            _grid.Dispose();
        }
    }
}
