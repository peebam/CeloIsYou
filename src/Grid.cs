using System;
using System.Collections.Generic;
using System.Linq;
using CeloIsYou.Extensions;
using CeloIsYou.Rules;

namespace CeloIsYou
{
    public class Grid : IDisposable
    {
        private class Cell : IDisposable
        {
            public readonly Coordinates Coordinates;
            public IReadOnlyList<Entity> Entities => _entities;
            private readonly List<Entity> _entities;

            public bool IsEmpty => _entities.Count == 0;

            public Cell(Coordinates coordinates)
            {
                Coordinates = coordinates;
                _entities = new List<Entity>();
            }

            public void Dispose()
            {
                _entities.Clear();
            }

            public void Enter(Entity entity)
            {
                _entities.Add(entity);
                entity.DrawOrder = _entities.Count;
            }

            public void Exit(Entity entity)
            {
                _entities.Remove(entity);
            }
        }

        private static IReadOnlyList<Entity> EmptyList = new List<Entity>();

        private readonly Dictionary<Entity, Cell> _cellsByEntities;
        private readonly Dictionary<Coordinates, Cell> _cellsByCoordinates;

        private readonly int _width;
        private readonly int _height;

        public Grid(int width, int height)
        {
            _width = width;
            _height = height;
            _cellsByCoordinates = new Dictionary<Coordinates, Cell>();
            _cellsByEntities = new Dictionary<Entity, Cell>();
        }

        public void Dispose()
        {
            foreach (var cell in _cellsByEntities.Values)
                cell.Dispose();

            _cellsByEntities.Clear();
        }

        public bool Enter(Entity entity, Coordinates to)
        {
            if (!HasCell(to))
                return false;

            var cell = EnterCell(entity, to);
            _cellsByEntities[entity] = cell;
            
            return true;
        }

        public bool Exit(Entity entity)
        {
            var cell = GetCell(entity);
            if (cell == null)
                return false;

            ExitCell(entity, cell);
            _cellsByEntities.Remove(entity);
            return true;
        }

        public List<Entity> GetEntities()
            => _cellsByEntities.Keys.ToList();

        public IReadOnlyList<Entity> GetEntities(Coordinates coordinates, Predicate<Entity> predicate)
            => GetCell(coordinates, createIfNotExists: false)?.Entities.Where(e => predicate(e))
                                                                       .ToList() ?? EmptyList;
        public bool HasEntities(Coordinates coordinates, Predicate<Entity> predicate)
            => GetCell(coordinates, createIfNotExists: false)?.Entities.Where(e => predicate(e))
                                                                       .Any() ?? false;

        public IEnumerable<Expression> GetExpressions()
        {
            var rulesHorizontal = GetExpressions(Direction.Right);
            var rulesVertical = GetExpressions(Direction.Down);
            return rulesHorizontal.Union(rulesVertical);
        }

        public bool IsMoveAllowed(Entity entity, Coordinates to)
        {
            if (!HasCell(to))
                return false;

            if (!_cellsByEntities.ContainsKey(entity))
                return false;

            return true;
        }

        public void Move(Entity entity, Coordinates to)
        {
            var fromCell = _cellsByEntities[entity];
            ExitCell(entity, fromCell);

            var toCell = EnterCell(entity, to);
            _cellsByEntities[entity] = toCell;
        }

        private Cell EnterCell(Entity entity, Coordinates to)
        {
            if (!HasCell(to))
                return null;

            var cell = GetCell(to, createIfNotExists: true);
            cell.Enter(entity);
            return cell;
        }

        private void ExitCell(Entity entity, Cell cell)
        {
            cell.Exit(entity);
            if (cell.IsEmpty)
                _cellsByCoordinates.Remove(cell.Coordinates);
        }
        private IEnumerable<Expression> GetExpressions(Direction direction)
        {
            var verbs = _cellsByEntities.Where(kv => kv.Key.Type.IsVerb())
                                               .OrderBy(kv => kv.Value.Coordinates.Y)
                                               .ThenBy(kv => kv.Value.Coordinates.X)
                                               .Select(kv => new { Cell = kv.Value, Entity = kv.Key })
                                               .ToList();

            var rules = new List<Expression>();
            foreach (var verb in verbs)
            {
                var leftCell = GetCell(verb.Cell.Coordinates, direction.Reverse());
                var rightCell = GetCell(verb.Cell.Coordinates, direction);
                if (leftCell == null || rightCell == null)
                    continue;

                var subjects = leftCell.Entities.Where(e => e.Type.IsNature());
                var objects = rightCell.Entities.Where(e => e.Type.IsNature() || e.Type.IsState());
                if (!subjects.Any() || !objects.Any())
                    continue;

                var rule = new Expression(subjects.First().Type, verb.Entity.Type, objects.First().Type);

                rules.Add(rule);
            }
            return rules;
        }

        private Cell GetCell(Entity entity)
        {
            if (!_cellsByEntities.ContainsKey(entity))
                return null;

            return _cellsByEntities[entity];
        }

        private Cell GetCell(Coordinates coordinates, bool createIfNotExists)
        {
            if (!HasCell(coordinates))
                return null;

            if (_cellsByCoordinates.ContainsKey(coordinates))
                return _cellsByCoordinates[coordinates];

            if (!createIfNotExists)
                return null;

            var cell = new Cell(coordinates);
            _cellsByCoordinates.Add(coordinates, cell);
            return cell;
        }

        private Cell GetCell(Coordinates coordinates, Direction direction)
        {
            if (!HasCell(coordinates, direction))
                return null;

            var coordinates_ = coordinates.Add(direction);
            if (!_cellsByCoordinates.ContainsKey(coordinates_))
                return null;

            return _cellsByCoordinates[coordinates_];
        }

        private bool HasCell(Coordinates coordinates)
            => coordinates.X >= 0 && coordinates.X < _width && coordinates.Y >= 0 && coordinates.Y < _height;

        private bool HasCell(Coordinates coordinates, Direction direction)
            => coordinates.TryAdd(direction, out var coordinates_) && HasCell(coordinates_);
    }
}
