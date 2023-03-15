using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class SettlerEnterSystem : IEcsInitSystem, IEcsRunSystem
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

            var pathEndFilter = world.Filter<PathEnd>().Inc<Settler>().Inc<GameObjectLink>().End();

            var gameObjectPool = world.GetPool<GameObjectLink>();
            var pathEndPool = world.GetPool<PathEnd>();
            var moveInPool = world.GetPool<MoveSettlerIn>();
            
            foreach (var pathEndEntity in pathEndFilter)
            {
                var endPosition = pathEndPool.Get(pathEndEntity).EndPosition;
                
                var gameObject = gameObjectPool.Get(pathEndEntity).Value;
                Object.Destroy(gameObject);
                world.DelEntity(pathEndEntity);

                var housePackedEntity = _mapGrid[endPosition];
                if (housePackedEntity.Unpack(out var packedWorld, out var entity))
                {
                    if (!moveInPool.Has(entity)) moveInPool.Add(entity);

                    ref var moveInComponent = ref moveInPool.Get(entity);

                    moveInComponent.Amount++;
                }
            }
        }
    }
}