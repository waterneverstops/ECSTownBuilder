using TownBuilder.SO;

namespace TownBuilder.Context
{
    public class LevelContext
    {
        public readonly MapGrid MapGrid;
        public readonly LevelDescription LevelDescription;

        public LevelContext(LevelDescription levelDescription)
        {
            LevelDescription = levelDescription;

            var mapSize = LevelDescription.MapSize;
            MapGrid = new MapGrid(mapSize.x, mapSize.y);
        }
    }
}