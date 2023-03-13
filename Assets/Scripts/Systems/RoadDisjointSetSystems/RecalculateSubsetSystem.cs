using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Building;
using TownBuilder.Components.Grid;
using TownBuilder.Components.RoadDisjointSet;
using TownBuilder.Context;
using TownBuilder.Context.DisjointSet;

namespace TownBuilder.Systems.RoadDisjointSetSystems
{
    public class RecalculateSubsetSystem : IEcsInitSystem, IEcsRunSystem
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

            var recalculateFilter = world.Filter<RecalculateSubset>().Inc<Road>().End();

            var cellPool = world.GetPool<Cell>();
            var roadPool = world.GetPool<Road>();

            foreach (var mergeEntity in recalculateFilter)
            {
                if (_roadDisjointSet[mergeEntity].Parent != null) continue;
                
                
            }
        }
    }
}