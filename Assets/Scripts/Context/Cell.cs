using Leopotam.EcsLite;

namespace TownBuilder.Context
{
    public class Cell
    {
        public CellType CellType = CellType.Empty;

        private EcsPackedEntityWithWorld _packedEntityWithWorld;

        public Cell(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            _packedEntityWithWorld = packedEntityWithWorld;
        }
    }
}