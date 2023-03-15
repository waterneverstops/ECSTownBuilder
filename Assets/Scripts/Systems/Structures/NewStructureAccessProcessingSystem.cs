using Leopotam.EcsLite;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Structures
{
    public class NewStructureAccessProcessingSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var structureFilter = world.Filter<Structure>().Inc<RoadAccessType>().Inc<NewGridBuilding>().End();

            var refreshPool = world.GetPool<RefreshRoadAccess>();
            
            foreach (var structureEntity in structureFilter)
            {
                if (!refreshPool.Has(structureEntity)) refreshPool.Add(structureEntity);
            }
        }
    }
}