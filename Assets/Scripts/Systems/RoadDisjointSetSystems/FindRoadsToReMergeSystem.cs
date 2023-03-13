using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Grid;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.Context.MapRoadDisjointSet;

namespace TownBuilder.Systems.RoadDisjointSetSystems
{
    public class FindRoadsToReMergeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;
        private RoadDisjointSet _roadDisjointSet;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
            _roadDisjointSet = _levelContextInjection.Value.RoadDisjointSet;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var destroyFilter = world.Filter<Road>().Inc<Destroy>().End();

            var cellPool = world.GetPool<Cell>();
            var roadPool = world.GetPool<Road>();
            var destroyPool = world.GetPool<Destroy>();
            var reMergePool = world.GetPool<ReMerge>();

            foreach (var destroyEntity in destroyFilter)
            {
                _roadDisjointSet.RemoveNode(destroyEntity);

                foreach (var neighbourPosition in _grid.GetNeighbours(cellPool.Get(destroyEntity).Position))
                    if (_grid[neighbourPosition].Unpack(out var packedWorld, out var entity))
                    {
                        if (!roadPool.Has(entity) || destroyPool.Has(entity)) continue;

                        _roadDisjointSet[entity].Parent = null;
                        reMergePool.Add(entity);
                    }
            }
        }
    }
}