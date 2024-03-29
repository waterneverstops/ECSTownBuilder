﻿using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.Systems.Building
{
    public class AreaBuilderSystem : IEcsInitSystem, IEcsRunSystem
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
            var mouseInputFilter = world.Filter<LeftMousePressed>().Inc<LeftMouseReleased>().End();

            foreach (var builderEntity in builderFilter)
            foreach (var mouseInputEntity in mouseInputFilter)
            {
                var pressedPool = world.GetPool<LeftMousePressed>();
                var releasedPool = world.GetPool<LeftMouseReleased>();
                var pressedPosition = pressedPool.Get(mouseInputEntity).Position;
                var releasedPosition = releasedPool.Get(mouseInputEntity).Position;

                var startPosition = new Vector2Int(Math.Min(pressedPosition.x, releasedPosition.x), Math.Min(pressedPosition.y, releasedPosition.y));
                var endPosition = new Vector2Int(Math.Max(pressedPosition.x, releasedPosition.x), Math.Max(pressedPosition.y, releasedPosition.y));
                
                for (var x = startPosition.x; x <= endPosition.x; x++)
                for (var y = startPosition.y; y <= endPosition.y; y++)
                {
                    if (!_mapGrid.IsPositionInbound(x, y) || !_mapGrid.IsPositionFree(x, y)) continue;

                    var builderPool = world.GetPool<Builder>();
                    var prefab = builderPool.Get(builderEntity).Prefab;

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