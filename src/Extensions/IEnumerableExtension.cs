
using CeloIsYou.Core;
using CeloIsYou.Rules;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CeloIsYou.src.Extensions
{
    public static class IEnumerableExtension
    {
        public static void ApplyAll(this IEnumerable<Rule> rules, IEnumerable<Entity> entities)
        {
            foreach (var rule in rules)
                rule.Apply(entities);
        }

        public static void ClearStatesAll(this IEnumerable<Entity> entities)
        {
            foreach(var entity in entities)
                entity.ClearStates();
        }

        public static void UpdateAll(this IEnumerable<IUpdatable> updatables, GameTime gameTime)
        {
            foreach (var updatable in updatables)
                updatable.Update(gameTime);
        }
    }
}
