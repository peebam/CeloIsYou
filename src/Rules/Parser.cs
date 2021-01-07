using System;
using System.Collections.Generic;
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
            [EntityTypes.ActionPush] = (e => e.IsPushable = true),
            [EntityTypes.ActionYou] = (e => e.IsControlled = true),
            [EntityTypes.ActionStop] = (e => e.IsStoping = true),
        };

        private static readonly Dictionary<EntityTypes, EntityTypes> _types = new()
        {
            [EntityTypes.NatureCelo] = EntityTypes.ObjectCelo,
            [EntityTypes.NatureRock] = EntityTypes.ObjectRock,
            [EntityTypes.NatureWall] = EntityTypes.ObjectWall,
        };

        public List<ICommand> Commands { get; private set; }
        public List<Rule> Rules { get; private set; }

        public Parser()
        {
            Commands = new List<ICommand>();
            Rules = new List<Rule>();
        }

        public void Process(IEnumerable<Expression> expressions, IEnumerable<Entity> entities)
        {
            Commands.Clear();
            Rules.Clear();

            foreach (var expression in expressions)
                ProcessCore(expression, entities);
        }

        private void ProcessCore(Expression expression, IEnumerable<Entity> entities)
        {
            if (!_predicates.TryGetValue(expression.Subject, out var predicate))
                return;

            if (_actions.TryGetValue(expression.Object, out var action))
            {
                var rule = new Rule(predicate, action);
                Rules.Add(rule);
                return;
            }

            if (_types.TryGetValue(expression.Object, out var type))
            {
                foreach (var entity in entities)
                {
                    if (!predicate(entity))
                        continue;

                    var command = new ChangeTypeCommand(entity, type);
                    Commands.Add(command);
                }
            }
        }
    }
}
