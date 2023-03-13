using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;
using TownBuilder.Context;

namespace TownBuilder.Systems.Building
{
    public class PathGhostBuilderSystem : IEcsInitSystem, IEcsRunSystem
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

            var leftMouseReleasedFilter = world.Filter<LeftMouseReleased>().End();
            var rightMousePressedFilter = world.Filter<RightMousePressed>().End();
            if (leftMouseReleasedFilter.GetEntitiesCount() > 0 || rightMousePressedFilter.GetEntitiesCount() > 0)
            {
                var destroyPool = world.GetPool<Destroy>();
                var ghostFilter = world.Filter<GhostBuilding>().End();

                foreach (var ghostEntity in ghostFilter) destroyPool.Add(ghostEntity);
                return;
            }

            var builderFilter = world.Filter<Builder>().Inc<BuildPath>().End();
            var mouseInputFilter = world.Filter<LeftMousePressed>().Inc<LeftMousePressing>().Exc<LeftMouseReleased>().End();

            foreach (var builderEntity in builderFilter)
            foreach (var mouseInputEntity in mouseInputFilter)
            {
                var pressedPool = world.GetPool<LeftMousePressed>();
                var pressingPool = world.GetPool<LeftMousePressing>();
                var pressedPosition = pressedPool.Get(mouseInputEntity).Position;
                var pressingPosition = pressingPool.Get(mouseInputEntity).Position;

                var previousPath = _gridPathfinder.Path;
                if (previousPath.Count > 0 && previousPath[0] == pressedPosition && previousPath[^1] == pressingPosition) continue;

                var destroyPool = world.GetPool<Destroy>();
                var ghostFilter = world.Filter<GhostBuilding>().End();

                foreach (var ghostEntity in ghostFilter) destroyPool.Add(ghostEntity);

                var path = _gridPathfinder.GetAStarSearchPath(_mapGrid, pressedPosition, pressingPosition);
                if (path.Count == 0) continue;

                var builderPool = world.GetPool<Builder>();
                var prefab = builderPool.Get(builderEntity).GhostPrefab;

                foreach (var buildPosition in path)
                {
                    if (!_mapGrid.IsPositionInbound(buildPosition) || !_mapGrid.IsPositionFree(buildPosition)) continue;

                    if (_mapGrid[buildPosition].Unpack(out var packedWorld, out var entity))
                        if (destroyPool.Has(entity))
                        {
                            destroyPool.Del(entity);
                            continue;
                        }

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