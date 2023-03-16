using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems
{
    public class GameObjectDestroySystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var destroyFilter = world.Filter<Destroy>().Inc<GameObjectLink>().Exc<Cell>().Exc<Indestructible>().End();

            var gameObjectPool = world.GetPool<GameObjectLink>();
            
            foreach (var destroyEntity in destroyFilter)
            {
                var gameObject = gameObjectPool.Get(destroyEntity).Value;
                Object.Destroy(gameObject);
                world.DelEntity(destroyEntity);
            }
        }
    }
}