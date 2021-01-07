using CeloIsYou.Enumerations;

namespace CeloIsYou.Rules
{
    public class Expression
    {
        public readonly EntityTypes Object;
        public readonly EntityTypes Subject;
        public readonly EntityTypes Verb;

        public Expression(EntityTypes subject, EntityTypes verb, EntityTypes @object)
        {
            Object = @object;
            Subject = subject;
            Verb = verb;
        }
    }
}
