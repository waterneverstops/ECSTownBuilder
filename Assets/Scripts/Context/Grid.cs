namespace TownBuilder.Context
{
    public class Grid
    {
        private readonly Cell[,] _cells;

        public int Width { get; }

        public int Height { get; }

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            _cells = new Cell[Width, Height];
        }

        public Cell this[int x, int y]
        {
            get => _cells[x, y];
            set => _cells[x, y] = value;
        }
    }
}