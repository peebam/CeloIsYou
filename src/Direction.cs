using System;

namespace CeloIsYou
{
    public class Direction
    {
        public static readonly Direction Left = new(-1, 0);
        public static readonly Direction Right = new(1, 0);
        public static readonly Direction Up = new(0, -1);
        public static readonly Direction Down = new(0, 1);

        public readonly int X;
        public readonly int Y;

        public Direction(int x, int y)
        {
            if (x < -1 || x > 1) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < -1 || y > 1) throw new ArgumentOutOfRangeException(nameof(y));

            X = x;
            Y = y;
        }

        public Direction(Direction direction)
        {
            X = direction.X;
            Y = direction.Y;
        }

        public Direction Reverse()
            => new(-X, -Y);
    }
}
