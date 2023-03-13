using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Context;
using UnityEngine;

namespace TownBuilder.Systems
{
    public class GridDestroySystem : IEcsInitSystem, IEcsRunSystem
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

            var destroyFilter = world.Filter<Destroy>().Inc<Cell>().End();

            foreach (var destroyEntity in destroyFilter)
            {
                var cellPool = world.GetPool<Cell>();
                var gameObjectPool = world.GetPool<GameObjectLink>();
                var destroyPosition = cellPool.Get(destroyEntity).Position;

                if (gameObjectPool.Has(destroyEntity))
                {
                    var gameObject = gameObjectPool.Get(destroyEntity);
                    Object.Destroy(gameObject.Value);
                }
                
                world.DelEntity(destroyEntity);
                
                var newEntity = world.NewEntity();
                ref var cellComponent = ref cellPool.Add(newEntity);
                cellComponent.Position = new Vector2Int(destroyPosition.x, destroyPosition.y);

                _mapGrid[destroyPosition] = world.PackEntityWithWorld(newEntity);
            }
        }
    }
}