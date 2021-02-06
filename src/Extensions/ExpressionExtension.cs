
using CeloIsYou.Enumerations;
using CeloIsYou.Rules;

namespace CeloIsYou.Extensions
{
    public static class ExpressionExtension
    {
        public static bool IsTypeChanging(this Expression expression)
            => expression.Verb == EntityTypes.VerbIs && expression.Object.IsNature();

        public static bool IsStateChanging(this Expression expression)
            => expression.Verb == EntityTypes.VerbIs && expression.Object.IsState();
    }
}
