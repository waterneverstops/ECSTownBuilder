using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Context;
using UnityEngine;

namespace TownBuilder.Systems
{
    public class GridDeleteSystem : IEcsInitSystem, IEcsRunSystem
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

            var deleteFilter = world.Filter<Delete>().Inc<Cell>().End();

            foreach (var deleteEntity in deleteFilter)
            {
                var cellPool = world.GetPool<Cell>();
                var gameObjectPool = world.GetPool<GameObjectLink>();
                var deletePosition = cellPool.Get(deleteEntity).Position;

                if (gameObjectPool.Has(deleteEntity))
                {
                    var gameObject = gameObjectPool.Get(deleteEntity);
                    Object.Destroy(gameObject.Value);
                }
                
                world.DelEntity(deleteEntity);
                
                var newEntity = world.NewEntity();
                ref var cellComponent = ref cellPool.Add(newEntity);
                cellComponent.Position = new Vector2Int(deletePosition.x, deletePosition.y);

                _mapGrid[deletePosition] = world.PackEntityWithWorld(newEntity);
            }
        }
    }
}