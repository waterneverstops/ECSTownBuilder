using System;

namespace TownBuilder.SO.RoadSetup
{
    [Serializable]
    public class RoadNeighborsAndVariant
    {
        public RoadNeighborsByDirections Neighbors;
        public ViewVariant ViewVariant;
    }
}