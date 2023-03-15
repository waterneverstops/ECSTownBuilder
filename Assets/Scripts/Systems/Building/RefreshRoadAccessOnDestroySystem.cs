using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;

namespace TownBuilder.Systems.Building
{
    public class RefreshRoadAccessOnDestroySystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var destroyFilter = world.Filter<Destroy>().Inc<Road>().End();

            var refreshPool = world.GetPool<RoadRefreshNeighbourAccess>();

            foreach (var newSpawnedRoad in destroyFilter)
            {
                if (refreshPool.Has(newSpawnedRoad)) continue;
                refreshPool.Add(newSpawnedRoad);
            }
        }
    }
}