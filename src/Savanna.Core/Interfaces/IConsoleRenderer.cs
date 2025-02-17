namespace Savanna.Core.Interfaces
{
    /// <summary>
    /// Defines a renderer for the console.
    /// </summary>
    public interface IConsoleRenderer
    {
        /// <summary>
        /// Renders the specified field.
        /// </summary>
        /// <param name="field">A two-dimensional character array representing the field layout.</param>
        public void RenderField(char[,] field);
    }
}
