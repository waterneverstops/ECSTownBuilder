﻿using Leopotam.EcsLite;
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

            var mouseReleasedFilter = world.Filter<MouseReleased>().End();
            foreach (var mouseReleasedEntity in mouseReleasedFilter)
            {
                var deletePool = world.GetPool<Delete>();
                var ghostFilter = world.Filter<GhostBuilding>().End();

                foreach (var ghostEntity in ghostFilter) deletePool.Add(ghostEntity);
                return;
            }

            var builderFilter = world.Filter<Builder>().Inc<BuildPath>().End();
            var mouseInputFilter = world.Filter<MousePressed>().Inc<MousePressing>().Exc<MouseReleased>().End();

            foreach (var builderEntity in builderFilter)
            foreach (var mouseInputEntity in mouseInputFilter)
            {
                var pressedPool = world.GetPool<MousePressed>();
                var pressingPool = world.GetPool<MousePressing>();
                var pressedPosition = pressedPool.Get(mouseInputEntity).Position;
                var pressingPosition = pressingPool.Get(mouseInputEntity).Position;

                var previousPath = _gridPathfinder.Path;
                if (previousPath.Count > 0 && previousPath[0] == pressedPosition && previousPath[^1] == pressingPosition) continue;

                var deletePool = world.GetPool<Delete>();
                var ghostFilter = world.Filter<GhostBuilding>().End();

                foreach (var ghostEntity in ghostFilter) deletePool.Add(ghostEntity);

                var path = _gridPathfinder.GetAStarSearchPath(_mapGrid, pressedPosition, pressingPosition);
                if (path.Count == 0) continue;

                var builderPool = world.GetPool<Builder>();
                var prefab = builderPool.Get(builderEntity).GhostPrefab;

                foreach (var buildPosition in path)
                {
                    if (!_mapGrid.IsPositionInbound(buildPosition) || !_mapGrid.IsPositionFree(buildPosition)) continue;

                    if (_mapGrid[buildPosition].Unpack(out var packedWorld, out var entity))
                        if (deletePool.Has(entity))
                        {
                            deletePool.Del(entity);
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