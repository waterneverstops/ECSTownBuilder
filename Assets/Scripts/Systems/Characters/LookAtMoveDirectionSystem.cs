using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Components.Links;

namespace TownBuilder.Systems.Characters
{
    public class LookAtMoveDirectionSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var lookFilter = world.Filter<LookAtMoveDirection>().Inc<Velocity>().Inc<GameObjectLink>().End();

            var gameObjectPool = world.GetPool<GameObjectLink>();
            var velocityPool = world.GetPool<Velocity>();
            
            foreach (var lookEntity in lookFilter)
            {
                var gameObject = gameObjectPool.Get(lookEntity).Value;
                var velocity = velocityPool.Get(lookEntity).Vector;
                
                gameObject.transform.LookAt(gameObject.transform.position + velocity);
            }
        }
    }
}