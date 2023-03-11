using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TownBuilder.Systems.Camera
{
    public sealed class CameraInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsCustomInject<InputActions> _inputActionsInjection = default;

        private EcsWorld _world;
        private InputActions _inputActions;

        private InputAction _rotationAction;
        private InputAction _movementAction;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _inputActions = _inputActionsInjection.Value;
            
            _rotationAction = _inputActions.CameraControl.CameraRotation;
            _movementAction = _inputActions.CameraControl.CameraMovement;

            _movementAction.started += OnCameraMovementActionStarted;
            _rotationAction.started += OnCameraRotationActionStarted;
            _movementAction.canceled += OnCameraMovementActionCanceled;
            _rotationAction.canceled += OnCameraRotationActionCanceled;
        }

        public void Destroy(IEcsSystems systems)
        {
            _movementAction.started -= OnCameraMovementActionStarted;
            _rotationAction.started -= OnCameraRotationActionStarted;
            _movementAction.canceled -= OnCameraMovementActionCanceled;
            _rotationAction.canceled -= OnCameraRotationActionCanceled;
        }

        public void Run(IEcsSystems systems)
        {
            var moveFilter = _world.Filter<MoveCamera>().End();
            var moveComponents = _world.GetPool<MoveCamera>();

            foreach (var entity in moveFilter)
            {
                ref var moveCamera = ref moveComponents.Get(entity);
                moveCamera.Value = _movementAction.ReadValue<Vector2>();
            }

            var rotateFilter = _world.Filter<RotateCamera>().End();
            var rotateComponents = _world.GetPool<RotateCamera>();

            foreach (var entity in rotateFilter)
            {
                ref var rotateCamera = ref rotateComponents.Get(entity);
                rotateCamera.Value = _rotationAction.ReadValue<float>();
            }
        }

        private void OnCameraRotationActionStarted(InputAction.CallbackContext context)
        {
            var newEntity = _world.NewEntity();
            var pool = _world.GetPool<RotateCamera>();

            ref var rotateComponent = ref pool.Add(newEntity);
            rotateComponent.Value = context.ReadValue<float>();
        }

        private void OnCameraMovementActionStarted(InputAction.CallbackContext context)
        {
            var newEntity = _world.NewEntity();
            var pool = _world.GetPool<MoveCamera>();

            ref var moveComponent = ref pool.Add(newEntity);
            moveComponent.Value = context.ReadValue<Vector2>();
        }

        private void OnCameraRotationActionCanceled(InputAction.CallbackContext context)
        {
            var filter = _world.Filter<RotateCamera>().End();
            var components = _world.GetPool<RotateCamera>();

            foreach (var entity in filter) components.Del(entity);
        }

        private void OnCameraMovementActionCanceled(InputAction.CallbackContext context)
        {
            var filter = _world.Filter<MoveCamera>().End();
            var components = _world.GetPool<MoveCamera>();

            foreach (var entity in filter) components.Del(entity);
        }
    }
}