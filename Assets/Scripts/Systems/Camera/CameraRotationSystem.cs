using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Links;
using TownBuilder.Components.Tags;
using UnityEngine;

namespace TownBuilder.Systems.Camera
{
    public sealed class CameraRotationSystem : IEcsRunSystem
    {
        private const float RotationSpeed = 60.0f;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var rotateFilter = world.Filter<RotateCamera>().End();
            var pivotFilter = world.Filter<CameraPivotTag>().Inc<GameObjectLink>().End();
            var rotateComponents = world.GetPool<RotateCamera>();
            var gameObjectComponents = world.GetPool<GameObjectLink>();

            foreach (var entity in rotateFilter)
            {
                ref var rotateCamera = ref rotateComponents.Get(entity);


                foreach (var pivotEntity in pivotFilter)
                {
                    var pivot = gameObjectComponents.Get(pivotEntity).Value;

                    pivot.transform.RotateAround(pivot.transform.position, Vector3.up, rotateCamera.Value * RotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}