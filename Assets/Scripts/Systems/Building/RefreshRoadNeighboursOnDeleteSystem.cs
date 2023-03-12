using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Context;

namespace TownBuilder.Systems.Building
{
    public class RefreshRoadNeighboursOnDeleteSystem : IEcsInitSystem, IEcsRunSystem
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

            var refreshFilter = world.Filter<Delete>().Inc<RefreshRoadNeighboursOnDelete>().End();

            var refreshPool = world.GetPool<RefreshRoadModel>();
            var cellPool = world.GetPool<Cell>();
            var roadPool = world.GetPool<Road>();
            var ghostPool = world.GetPool<GhostRoad>();

            foreach (var refreshRoad in refreshFilter)
            foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(refreshRoad).Position))
                if (_grid[neighbourPosition].Unpack(out world, out var entity))
                {
                    if (!(roadPool.Has(entity) || ghostPool.Has(entity)) || refreshPool.Has(entity)) continue;

                    refreshPool.Add(entity);
                }
        }
    }
}