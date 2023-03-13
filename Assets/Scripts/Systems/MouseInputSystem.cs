using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Input;
using TownBuilder.Components.Links;
using TownBuilder.Components.Tags;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TownBuilder.Systems
{
    public class MouseInputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private const string GroundLayerName = "Ground";

        private readonly EcsCustomInject<InputActions> _inputActionsInjection = default;

        private EcsWorld _world;
        private InputActions _inputActions;

        private InputAction _leftMousePressed;
        private InputAction _rightMousePressed;
        private InputAction _mousePosition;

        private LayerMask _groundMask;

        private bool _isPointerOverUI;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _inputActions = _inputActionsInjection.Value;

            _leftMousePressed = _inputActions.MouseControl.LeftMousePressed;
            _rightMousePressed = _inputActions.MouseControl.RightMousePressed;
            _mousePosition = _inputActions.MouseControl.MousePosition;

            _leftMousePressed.started += OnLeftMousePressedStarted;
            _rightMousePressed.started += OnRightMousePressedStarted;

            _groundMask = LayerMask.GetMask(GroundLayerName);
        }

        public void Destroy(IEcsSystems systems)
        {
            _leftMousePressed.started -= OnLeftMousePressedStarted;
            _rightMousePressed.started -= OnRightMousePressedStarted;
        }

        public void Run(IEcsSystems systems)
        {
            _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
            
            ProcessMouseInput<LeftMousePressed, LeftMousePressing, LeftMouseReleased>(_leftMousePressed);
            ProcessMouseInput<RightMousePressed, RightMousePressing, RightMouseReleased>(_rightMousePressed);
        }

        private void ProcessMouseInput<TPressed, TPressing, TReleased>(InputAction mousePressedAction) 
            where TPressed : struct, IMouseInput
            where TPressing : struct, IMouseInput
            where TReleased : struct, IMouseInput
        {
            // Clear mouse input components on next frame from releasing
            var releaseFilter = _world.Filter<TReleased>().End();
            foreach (var entity in releaseFilter) _world.DelEntity(entity);

            var pressingFilter = _world.Filter<TPressing>().End();
            var pressingPool = _world.GetPool<TPressing>();

            foreach (var entity in pressingFilter)
            {
                var mousePosition = _mousePosition.ReadValue<Vector2>();
                var raycastPosition = RaycastGround(mousePosition);
                if (raycastPosition == null) continue;

                if (_mousePosition.IsPressed()) pressingPool.Get(entity).Position = raycastPosition.Value;
                if (mousePressedAction.WasPressedThisFrame())
                {
                    var pressedPool = _world.GetPool<TPressed>();
                    ref var pressedComponent = ref pressedPool.Add(entity);

                    pressedComponent.Position = raycastPosition.Value;
                }

                if (!mousePressedAction.IsPressed())
                {
                    if (mousePressedAction.WasReleasedThisFrame())
                    {
                        var releasedPool = _world.GetPool<TReleased>();
                        ref var releasedComponent = ref releasedPool.Add(entity);

                        releasedComponent.Position = raycastPosition.Value;
                    }
                    else
                    {
                        // If mouse was released somewhere outside game focus
                        var pressedFilter = _world.Filter<TPressed>().End();
                        foreach (var pressedEntity in pressedFilter) _world.DelEntity(pressedEntity);
                    }
                }
            }
        }

        private void OnLeftMousePressedStarted(InputAction.CallbackContext obj)
        {
            if (_isPointerOverUI) return;
            
            var mousePosition = _mousePosition.ReadValue<Vector2>();
            var raycastPosition = RaycastGround(mousePosition);
            if (raycastPosition == null) return;

            var newEntity = _world.NewEntity();
            var pool = _world.GetPool<LeftMousePressing>();

            ref var pressingComponent = ref pool.Add(newEntity);
            pressingComponent.Position = raycastPosition.Value;
        }
        
        private void OnRightMousePressedStarted(InputAction.CallbackContext obj)
        {
            if (_isPointerOverUI) return;
            
            var mousePosition = _mousePosition.ReadValue<Vector2>();
            var raycastPosition = RaycastGround(mousePosition);
            if (raycastPosition == null) return;

            var newEntity = _world.NewEntity();
            var pool = _world.GetPool<RightMousePressing>();

            ref var pressingComponent = ref pool.Add(newEntity);
            pressingComponent.Position = raycastPosition.Value;
        }

        private Vector2Int? RaycastGround(Vector2 mousePosition)
        {
            var cameraFilter = _world.Filter<MainCameraTag>().Inc<GameObjectLink>().End();
            var gameObjectComponents = _world.GetPool<GameObjectLink>();

            foreach (var entity in cameraFilter)
            {
                var cameraGameObject = gameObjectComponents.Get(entity).Value;
                var camera = cameraGameObject.GetComponent<UnityEngine.Camera>();

                RaycastHit hit;
                var ray = camera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundMask))
                {
                    var positionInt = Vector3Int.FloorToInt(hit.point);
                    var gridPositionInt = new Vector2Int(positionInt.x, positionInt.z);
                    return gridPositionInt;
                }
            }

            return null;
        }
    }
}