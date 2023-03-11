using TownBuilder.SO;

namespace TownBuilder.Context
{
    public class LevelContext
    {
        public readonly Grid Grid;
        public readonly LevelDescription LevelDescription;

        public LevelContext(LevelDescription levelDescription)
        {
            LevelDescription = levelDescription;

            var mapSize = LevelDescription.MapSize;
            Grid = new Grid(mapSize.x, mapSize.y);
        }
    }
}