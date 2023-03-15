using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Links;

namespace TownBuilder.Systems.Characters
{
    public class MoveGameObjectSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var velocityFilter = world.Filter<Velocity>().Inc<Movable>().Inc<GameObjectLink>().End();

            var gameObjectPool = world.GetPool<GameObjectLink>();
            var velocityPool = world.GetPool<Velocity>();
            var movablePool = world.GetPool<Movable>();
            
            foreach (var velocityEntity in velocityFilter)
            {
                var gameObject = gameObjectPool.Get(velocityEntity).Value;
                var velocity = velocityPool.Get(velocityEntity).Vector;
                
                gameObject.transform.Translate(velocity);

                ref var movableComponent = ref movablePool.Get(velocityEntity);
                movableComponent.Position = gameObject.transform.position;
            }
        }
    }
}