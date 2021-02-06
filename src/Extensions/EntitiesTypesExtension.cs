using System;
using CeloIsYou.Enumerations;

namespace CeloIsYou.Extensions
{
    public static class EntityTypesExtension
    {
        public static string ToContentName(this EntityTypes type)
            => type switch
            {
                EntityTypes.StateKill => "States/kill",
                EntityTypes.StatePush => "States/push",
                EntityTypes.StateStop => "States/stop",
                EntityTypes.StateWeak => "States/weak",
                EntityTypes.StateWin => "States/win",

                EntityTypes.ActionYou => "States/you",

                EntityTypes.ObjectBox => "Objects/box",
                EntityTypes.ObjectCelo => "Objects/player",
                EntityTypes.ObjectRock => "Objects/rock",
                EntityTypes.ObjectSpot => "Objects/spot",
                EntityTypes.ObjectWall => "Objects/wall",

                EntityTypes.VerbIs => "Verbs/is",

                EntityTypes.NatureCelo => "Natures/celo",
                EntityTypes.NatureRock => "Natures/rock",
                EntityTypes.NatureSpot => "Natures/spot",
                EntityTypes.NatureText => "Natures/text",
                EntityTypes.NatureWall => "Natures/wall",
                _ => throw new ArgumentOutOfRangeException()
            };

        public static bool IsState(this EntityTypes type)
            => type == EntityTypes.StatePush || type == EntityTypes.ActionYou
                                              || type == EntityTypes.StateStop
                                              || type == EntityTypes.StateKill
                                              || type == EntityTypes.StateWeak
                                              || type == EntityTypes.StateWin;

        public static bool IsNature(this EntityTypes type)
            => type == EntityTypes.NatureCelo || type == EntityTypes.NatureRock
                                               || type == EntityTypes.NatureText
                                               || type == EntityTypes.NatureWall;

        public static bool IsVerb(this EntityTypes type)
            => type == EntityTypes.VerbIs;
    }
}
