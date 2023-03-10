using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Links;
using TownBuilder.Components.Tags;
using UnityEngine;

namespace TownBuilder.Systems.Camera
{
    public sealed class CameraMovementSystem : IEcsRunSystem
    {
        private const float MoveSpeed = 10.0f;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<MoveCamera>().End();
            var pivotFilter = world.Filter<CameraPivotTag>().Inc<GameObjectLink>().End();
            var cameraFilter = world.Filter<MainCameraTag>().Inc<GameObjectLink>().End();
            var moveComponents = world.GetPool<MoveCamera>();
            var gameObjectComponents = world.GetPool<GameObjectLink>();


            foreach (var entity in filter)
            {
                ref var moveCamera = ref moveComponents.Get(entity);

                foreach (var pivotEntity in pivotFilter)
                {
                    var pivot = gameObjectComponents.Get(pivotEntity).Value;

                    foreach (var cameraEntity in cameraFilter)
                    {
                        var camera = gameObjectComponents.Get(cameraEntity).Value;

                        var pivotTransform = pivot.transform;
                        var cameraTransform = camera.transform;
                        pivotTransform.localPosition += cameraTransform.right * moveCamera.Value.x * MoveSpeed * Time.deltaTime;
                        pivotTransform.localPosition += cameraTransform.up * moveCamera.Value.y * MoveSpeed * Time.deltaTime;
                    }
                }
            }
        }
    }
}