using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class FollowPathSystem : IEcsRunSystem
    {
        private const float CharacterSpeed = 2f;
        private const float PositionThreshold = 0.25f;
        private const float PositionOffset = 0.5f;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var pathFilter = world.Filter<Path>().Inc<Movable>().End();

            var pathPool = world.GetPool<Path>();
            var movablePool = world.GetPool<Movable>();
            var endPool = world.GetPool<PathEnd>();
            var velocityPool = world.GetPool<Velocity>();

            foreach (var pathEntity in pathFilter)
            {
                var path = pathPool.Get(pathEntity).Points;
                var currentPoint = path[^1];
                var currentPointPosition = new Vector3(currentPoint.x + PositionOffset, 0f, currentPoint.y + PositionOffset);
                var currentPosition = movablePool.Get(pathEntity).Position;

                var distance = Vector3.Distance(currentPosition, currentPointPosition);
                if (distance < PositionThreshold)
                {
                    path.RemoveAt(path.Count - 1);

                    if (path.Count == 0)
                    {
                        ref var endComponent = ref endPool.Add(pathEntity);
                        endComponent.EndPosition = currentPoint;
                        pathPool.Del(pathEntity);
                        continue;
                    }
                    
                    currentPoint = path[^1];
                    currentPointPosition = new Vector3(currentPoint.x + PositionOffset, 0f, currentPoint.y + PositionOffset);
                }

                var heading = currentPointPosition - currentPosition;
                var velocity = heading / distance * CharacterSpeed * Time.deltaTime;
                ref var velocityComponent = ref velocityPool.Add(pathEntity);
                velocityComponent.Vector = velocity;
            }
        }
    }
}