using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Building;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;

namespace TownBuilder.Systems.Building
{
    public class TriggerRefreshRoadAccessSystem : IEcsInitSystem, IEcsRunSystem
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

            var refreshFilter = world.Filter<TriggerRefreshRoadAccess>().End();

            var refreshPool = world.GetPool<RefreshRoadAccess>();
            var cellPool = world.GetPool<Cell>();
            var structurePool = world.GetPool<Structure>();

            foreach (var refreshEntity in refreshFilter)
            {
                foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(refreshEntity).Position, true))
                    if (_grid[neighbourPosition].Unpack(out var packedWorld, out var entity))
                    {
                        if (!structurePool.Has(entity) || refreshPool.Has(entity)) continue;

                        refreshPool.Add(entity);
                    }
            }
        }
    }
}