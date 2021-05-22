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
            [EntityTypes.NatureSpot] = (e => e.Type == EntityTypes.ObjectSpot),
            [EntityTypes.NatureText] = (e => e.Type.IsNature() || e.Type.IsState() || e.Type.IsVerb()),
            [EntityTypes.NatureWall] = (e => e.Type == EntityTypes.ObjectWall),
        };

        private static readonly Dictionary<EntityTypes, Action<Entity>> _states = new()
        {
            [EntityTypes.StateKill] = (e => e.IsKilling = true),
            [EntityTypes.StatePush] = (e => e.IsPushable = true),
            [EntityTypes.StateStop] = (e => e.IsStoping = true),
            [EntityTypes.StateWeak] = (e => e.IsWeak = true),
            [EntityTypes.StateWin] = (e => e.IsWin = true),
            [EntityTypes.ActionYou] = (e => e.IsControlled = true),
        };

        private static readonly Dictionary<EntityTypes, EntityTypes> _types = new()
        {
            [EntityTypes.NatureCelo] = EntityTypes.ObjectCelo,
            [EntityTypes.NatureRock] = EntityTypes.ObjectRock,
            [EntityTypes.NatureWall] = EntityTypes.ObjectWall,
            [EntityTypes.NatureSpot] = EntityTypes.ObjectSpot,
        };

        private Resources _resources;

        public Parser(Resources resources)
        {
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public Result Process(IEnumerable<Expression> expressions, IEnumerable<Entity> entities)
        {
            var rules = ProcessStateChangingExpression(expressions);
            var commands = ProcessTypeChangingExpression(expressions, entities);

            return new Result(commands, rules);
        }

        private List<Rule> ProcessStateChangingExpression(IEnumerable<Expression> expressions)
        {
            var rules = new List<Rule>();
            var stateChangingExpressions = expressions.Where(e => e.IsStateChanging());

            foreach (var expression in stateChangingExpressions)
            {
                if (!_predicates.TryGetValue(expression.Subject, out var predicate))
                    continue;

                if (!_states.TryGetValue(expression.Object, out var state))
                    continue;

                var rule = new Rule(predicate, state);
                rules.Add(rule);
            }

            return rules;
        }

        private List<ICommand> ProcessTypeChangingExpression(IEnumerable<Expression> expressions, IEnumerable<Entity> entities)
        {
            var commands = new List<ICommand>();
            var typeChangingExpressions = expressions.Where(e => e.IsTypeChanging()
                                                              && _predicates.ContainsKey(e.Subject)
                                                              && _types.ContainsKey(e.Object)).GroupBy(e => e.Subject);

            foreach (var group in typeChangingExpressions)
            {
                var predicate = _predicates[group.Key];
                var entitiesTypeToChange = entities.Where(e => predicate(e));

                foreach (var entity in entitiesTypeToChange)
                {
                    var commandExit = new ExitGameCommand(entity);
                    commands.Add(commandExit);

                    foreach(var expression in group)
                    {
                        var newType = _types[expression.Object];
                        var newEntity = new Entity(_resources, newType);
                        var commandEnter = new EnterGameCommand(newEntity, entity.Coordinates);
                        commands.Add(commandEnter);
                    }
                }
            }
            return commands;
        }
    }
}
