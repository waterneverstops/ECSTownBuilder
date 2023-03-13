using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;
using TownBuilder.Context;

namespace TownBuilder.Systems.Building
{
    public class PathBuilderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _mapGrid;
        private MapGridPathfinder _gridPathfinder;

        public void Init(IEcsSystems systems)
        {
            _mapGrid = _levelContextInjection.Value.MapGrid;
            _gridPathfinder = new MapGridPathfinder();
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var builderFilter = world.Filter<Builder>().Inc<BuildPath>().End();
            var mouseInputFilter = world.Filter<LeftMousePressed>().Inc<LeftMouseReleased>().End();

            foreach (var builderEntity in builderFilter)
            foreach (var mouseInputEntity in mouseInputFilter)
            {
                var pressedPool = world.GetPool<LeftMousePressed>();
                var releasedPool = world.GetPool<LeftMouseReleased>();
                var pressedPosition = pressedPool.Get(mouseInputEntity).Position;
                var releasedPosition = releasedPool.Get(mouseInputEntity).Position;

                var path = _gridPathfinder.GetAStarSearchPath(_mapGrid, pressedPosition, releasedPosition);
                if (path.Count == 0) continue;

                var builderPool = world.GetPool<Builder>();
                var prefab = builderPool.Get(builderEntity).Prefab;

                foreach (var buildPosition in path)
                {
                    if (!_mapGrid.IsPositionInbound(buildPosition) || !_mapGrid.IsPositionFree(buildPosition)) continue;
   
                    var spawnEntity = world.NewEntity();
                    var spawnPool = world.GetPool<SpawnPrefabGrid>();
                    ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                    spawnComponent.Position = buildPosition;
                    spawnComponent.Prefab = prefab;
                }
            }
        }
    }
}