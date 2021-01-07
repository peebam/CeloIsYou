using System;
using CeloIsYou.Enumerations;

namespace CeloIsYou.Extensions
{
    public static class EntityTypesExtension
    {
        public static string ToContentName(this EntityTypes type)
            => type switch
            {

                EntityTypes.ActionYou => "Actions/you",
                EntityTypes.ActionPush => "Actions/push",
                EntityTypes.ActionStop => "Actions/stop",

                EntityTypes.ObjectBox => "Objects/box",
                EntityTypes.ObjectCelo => "Objects/player",
                EntityTypes.ObjectRock => "Objects/rock",
                EntityTypes.ObjectSpot => "Objects/spot",
                EntityTypes.ObjectWall => "Objects/wall",

                EntityTypes.VerbIs => "Verbs/is",

                EntityTypes.NatureCelo => "Natures/celo",
                EntityTypes.NatureRock => "Natures/rock",
                EntityTypes.NatureText => "Natures/text",
                EntityTypes.NatureWall => "Natures/wall",
                _ => throw new ArgumentOutOfRangeException()
            };

        public static bool IsAction(this EntityTypes type)
            => type == EntityTypes.ActionPush || type == EntityTypes.ActionYou
                                              || type == EntityTypes.ActionStop;

        public static bool IsNature(this EntityTypes type)
            => type == EntityTypes.NatureCelo || type == EntityTypes.NatureRock
                                               || type == EntityTypes.NatureText
                                               || type == EntityTypes.NatureWall;

        public static bool IsVerb(this EntityTypes type)
            => type == EntityTypes.VerbIs;
    }
}
