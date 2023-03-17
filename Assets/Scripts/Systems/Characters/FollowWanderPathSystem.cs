using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components;
using TownBuilder.Components.Characters;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using UnityEngine;

namespace TownBuilder.Systems.Characters
{
    public class FollowWanderPathSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float CharacterSpeed = 2f;
        private const float PositionThreshold = 0.25f;
        private const float PositionOffset = 0.5f;

        private readonly EcsCustomInject<LevelContext> _levelContextInjection = default;

        private MapGrid _grid;

        public void Init(IEcsSystems systems)
        {
            _grid = _levelContextInjection.Value.MapGrid;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var pathFilter = world.Filter<WanderPath>().Inc<Movable>().End();

            var pathPool = world.GetPool<WanderPath>();
            var movablePool = world.GetPool<Movable>();
            var endPool = world.GetPool<WanderPathEnd>();
            var velocityPool = world.GetPool<Velocity>();
            var stepPool = world.GetPool<WanderStep>();

            foreach (var pathEntity in pathFilter)
            {
                ref var pathComponent = ref pathPool.Get(pathEntity);

                var currentPoint = pathComponent.NextStep;
                var currentPointPosition = new Vector3(currentPoint.x + PositionOffset, 0f, currentPoint.y + PositionOffset);
                var currentPosition = movablePool.Get(pathEntity).Position;

                var distance = Vector3.Distance(currentPosition, currentPointPosition);
                if (distance < PositionThreshold)
                {
                    pathComponent.StepsLeft--;
                    ref var stepComponent = ref stepPool.Add(pathEntity);
                    stepComponent.Position = currentPoint;
                    
                    var neighbours = _grid.GetPathfindingNeighbours(currentPoint, PathType.Road);
                    if (neighbours.Contains(pathComponent.LastStep)) neighbours.Remove(pathComponent.LastStep);
                    
                    pathComponent.LastStep = currentPoint;

                    if (pathComponent.StepsLeft == 0 || neighbours.Count == 0)
                    {
                        ref var endComponent = ref endPool.Add(pathEntity);
                        endComponent.EndPosition = currentPoint;
                        pathPool.Del(pathEntity);
                        continue;
                    }

                    var randomNeighbour = Random.Range(0, neighbours.Count);
                    currentPoint = neighbours[randomNeighbour];
                    pathComponent.NextStep = currentPoint;
                    currentPointPosition = new Vector3(currentPoint.x + PositionOffset, 0f, currentPoint.y + PositionOffset);
                    distance = Vector3.Distance(currentPosition, currentPointPosition);
                }

                var heading = currentPointPosition - currentPosition;
                var velocity = heading / distance * CharacterSpeed * Time.deltaTime;
                ref var velocityComponent = ref velocityPool.Add(pathEntity);
                velocityComponent.Vector = velocity;
            }
        }
    }
}