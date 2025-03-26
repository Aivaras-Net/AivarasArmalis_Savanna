namespace Savanna.Domain
{
    /// <summary>
    /// Represents a coordinate in a two-dimensional space.
    /// </summary>
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Position Null => new Position(-1, -1);

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the Euclidean distance to another position.
        /// </summary>
        /// <param name="other">The target position for distance measurement.</param>
        /// <returns>The Euclidean distance between the current and specified positions.</returns>
        public double DistanceTo(Position other)
        {
            int dx = X - other.X;
            int dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString() => $"({X}:{Y})";

        public override bool Equals(object? obj)
        {
            if (obj is Position other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Position? left, Position? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Position? left, Position? right)
        {
            return !(left == right);
        }
    }
}
