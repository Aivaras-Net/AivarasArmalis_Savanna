namespace Savanna.Core.Domain
{
    public class Position
    {
        public int X {  get; set; }
        public int Y { get; set; }

        public Position (int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Position other)
        {
            int dx = X - other.X;
            int dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public override string ToString() => $"({X}:{Y})";
    }
}
