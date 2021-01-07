using System;
using System.Collections.Generic;
using System.Linq;

namespace CeloIsYou.Rules
{
    public class Rule
    {
        private readonly Action<Entity> _action;
        private readonly Predicate<Entity> _predicate;

        public Rule(Predicate<Entity> predicate, Action<Entity> action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }

        public void Apply(IEnumerable<Entity> entities)
        {
            var subjects = entities.Where(e => _predicate(e));
            foreach (var subject in subjects)
                _action(subject);
        }
    }
}
