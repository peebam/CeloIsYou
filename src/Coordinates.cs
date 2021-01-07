using System;

namespace CeloIsYou
{
    public class Coordinates
    {
        public const int MinCoordinateX = -999;
        public const int MinCoordinateY = -999;
        public const int MaxCoordinateX = 999;
        public const int MaxCoordinateY = 999;

        public const int RangeX = MaxCoordinateX - MinCoordinateX + 1;
        public const int RangeY = MaxCoordinateY - MinCoordinateY + 1;

        public readonly int X;
        public readonly int Y;

        public Coordinates(int x, int y)
        {
            if (x < MinCoordinateX || x > MaxCoordinateX) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < MinCoordinateY || y > MaxCoordinateY) throw new ArgumentOutOfRangeException(nameof(y));

            X = x;
            Y = y;
        }

        public Coordinates Add(Direction direction)
             => new(X + direction.X, Y + direction.Y);

        public override bool Equals(object obj)
        {
            if (obj is Coordinates coordinates)
                return coordinates.X == X && coordinates.Y == Y;
            return false;
        }

        public override int GetHashCode() => X * RangeX + Y;

        public bool TryAdd(Direction direction, out Coordinates result)
        {
            var newX = X + direction.X;
            var newY = Y + direction.Y;

            if (newX < MinCoordinateX || newX > MaxCoordinateX || newY < MinCoordinateY || newY > MaxCoordinateY)
            {
                result = null;
                return false;
            }

            result = new Coordinates(newX, newY);
            return true;
        }
    }
}
