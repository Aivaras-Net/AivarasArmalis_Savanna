namespace Savanna.Core.Domain
{
    /// <summary>
    /// Represents a coordinate in a two-dimensional space.
    /// </summary>
    public class Position
    {
        public int X {  get; set; }
        public int Y { get; set; }

        public Position (int x, int y)
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
    }
}
