using System;
using System.Collections.Generic;
using System.Linq;
using CeloIsYou.Commands;
using CeloIsYou.Enumerations;
using CeloIsYou.Extensions;

namespace CeloIsYou.Rules
{
    public class Parser
    {
        private static readonly Dictionary<EntityTypes, Predicate<Entity>> _predicates = new()
        {
            [EntityTypes.NatureCelo] = (e => e.Type == EntityTypes.ObjectCelo),
            [EntityTypes.NatureRock] = (e => e.Type == EntityTypes.ObjectRock),
            [EntityTypes.NatureText] = (e => e.Type.IsNature() || e.Type.IsAction() || e.Type.IsVerb()),
            [EntityTypes.NatureWall] = (e => e.Type == EntityTypes.ObjectWall),
        };

        private static readonly Dictionary<EntityTypes, Action<Entity>> _actions = new()
        {
            [EntityTypes.ActionKill] = (e => e.IsKilling = true),
            [EntityTypes.ActionPush] = (e => e.IsPushable = true),
            [EntityTypes.ActionStop] = (e => e.IsStoping = true),
            [EntityTypes.ActionYou] = (e => e.IsControlled = true),
        };

        private static readonly Dictionary<EntityTypes, EntityTypes> _types = new()
        {
            [EntityTypes.NatureCelo] = EntityTypes.ObjectCelo,
            [EntityTypes.NatureRock] = EntityTypes.ObjectRock,
            [EntityTypes.NatureWall] = EntityTypes.ObjectWall,
        };

        public Parser()
        {
        }

        public Result Process(IEnumerable<Expression> expressions, IEnumerable<Entity> entities, Grid grid)
        {
            
            var commands = new List<ICommand>();
            var rules = new List<Rule>();

            foreach (var expression in expressions)
                ProcessCore(expression, entities, grid, commands, rules);

            return new Result(commands, rules);
        }

        private void ProcessCore(Expression expression, IEnumerable<Entity> entities, Grid grid, List<ICommand> commands, List<Rule> rules)
        {
            if (!_predicates.TryGetValue(expression.Subject, out var predicate))
                return;

            if (_actions.TryGetValue(expression.Object, out var action))
            {
                var rule = new Rule(predicate, action);
                rules.Add(rule);
                return;
            }
            
            if (_types.TryGetValue(expression.Object, out var type))
            {
                entities = entities.Where(e => predicate(e));
                foreach (var entity in entities)
                {

                    var coordinates = grid.GetCoordinates(entity);
                    var commandExit = new ExitGameCommand(entity, coordinates);
                    commands.Add(commandExit);

                    var newEntity = new Entity(type);
                    var commandEnter = new EnterGameCommand(newEntity, coordinates);
                    commands.Add(commandEnter);
                }
            }
        }
    }
}
