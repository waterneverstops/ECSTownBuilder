using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.Context.MapRoadDisjointSet;

namespace TownBuilder.Systems.RoadDisjointSetSystems
{
    public class MergeRoadsRefreshAccessSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var mergeFilter = world.Filter<Merge>().End();

            var refreshPool = world.GetPool<RoadRefreshNeighbourAccess>();
            
            foreach (var mergeEntity in mergeFilter)
            {
                if (refreshPool.Has(mergeEntity)) continue;
                refreshPool.Add(mergeEntity);
            }
        }
    }
}