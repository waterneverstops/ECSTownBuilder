using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Building
{
    public class AreaGhostBuilderSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _mapGrid;

        public void Init(IEcsSystems systems)
        {
            _mapGrid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var builderFilter = world.Filter<Builder>().Inc<BuildArea>().End();
            var mouseInputFilter = world.Filter<LeftMousePressed>().Inc<LeftMousePressing>().Exc<LeftMouseReleased>().End();

            foreach (var builderEntity in builderFilter)
            foreach (var mouseInputEntity in mouseInputFilter)
            {
                var pressedPool = world.GetPool<LeftMousePressed>();
                var pressingPool = world.GetPool<LeftMousePressing>();
                var pressedPosition = pressedPool.Get(mouseInputEntity).Position;
                var pressingPosition = pressingPool.Get(mouseInputEntity).Position;

                var destroyPool = world.GetPool<Destroy>();
                var ghostFilter = world.Filter<GhostBuilding>().End();

                foreach (var ghostEntity in ghostFilter) destroyPool.Add(ghostEntity);

                var builderPool = world.GetPool<Builder>();
                var prefab = builderPool.Get(builderEntity).GhostPrefab;

                var startPosition = new Vector2Int(Math.Min(pressedPosition.x, pressingPosition.x), Math.Min(pressedPosition.y, pressingPosition.y));
                var endPosition = new Vector2Int(Math.Max(pressedPosition.x, pressingPosition.x), Math.Max(pressedPosition.y, pressingPosition.y));

                for (var x = startPosition.x; x <= endPosition.x; x++)
                for (var y = startPosition.y; y <= endPosition.y; y++)
                {
                    if (!_mapGrid.IsPositionInbound(x, y) || !_mapGrid.IsPositionFree(x, y)) continue;

                    if (_mapGrid[x, y].Unpack(out var packedWorld, out var entity))
                        if (destroyPool.Has(entity))
                        {
                            destroyPool.Del(entity);
                            continue;
                        }

                    var spawnEntity = world.NewEntity();
                    var spawnPool = world.GetPool<SpawnPrefabGrid>();
                    ref var spawnComponent = ref spawnPool.Add(spawnEntity);
                    spawnComponent.Position = new Vector2Int(x, y);
                    spawnComponent.Prefab = prefab;
                }
            }
        }
    }
}