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
        private const string GroundMaskName = "Ground";

        private readonly EcsCustomInject<InputActions> _inputActionsInjection = default;

        private EcsWorld _world;
        private InputActions _inputActions;

        private InputAction _mousePressed;
        private InputAction _mousePosition;

        private LayerMask _groundMask;

        private bool _isPointerOverUI;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _inputActions = _inputActionsInjection.Value;

            _mousePressed = _inputActions.MouseControl.MousePressed;
            _mousePosition = _inputActions.MouseControl.MousePosition;

            _mousePressed.started += OnMousePressedStarted;

            _groundMask = LayerMask.GetMask(GroundMaskName);
        }

        public void Destroy(IEcsSystems systems)
        {
            _mousePressed.started -= OnMousePressedStarted;
        }

        public void Run(IEcsSystems systems)
        {
            _isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
            
            // Clear mouse input components on next frame from releasing
            var releaseFilter = _world.Filter<MouseReleased>().End();
            foreach (var entity in releaseFilter) _world.DelEntity(entity);

            var moveFilter = _world.Filter<MousePressing>().End();
            var pressingComponents = _world.GetPool<MousePressing>();

            foreach (var entity in moveFilter)
            {
                var mousePosition = _mousePosition.ReadValue<Vector2>();
                var raycastPosition = RaycastGround(mousePosition);
                if (raycastPosition == null) continue;

                if (_mousePosition.IsPressed()) pressingComponents.Get(entity).Position = raycastPosition.Value;
                if (_mousePressed.WasPressedThisFrame())
                {
                    var pressedPool = _world.GetPool<MousePressed>();
                    ref var pressedComponent = ref pressedPool.Add(entity);

                    pressedComponent.Position = raycastPosition.Value;
                }

                if (!_mousePressed.IsPressed())
                {
                    if (_mousePressed.WasReleasedThisFrame())
                    {
                        var releasedPool = _world.GetPool<MouseReleased>();
                        ref var releasedComponent = ref releasedPool.Add(entity);

                        releasedComponent.Position = raycastPosition.Value;
                    }
                    else
                    {
                        // If mouse was released somewhere outside game focus
                        var pressedFilter = _world.Filter<MousePressed>().End();
                        foreach (var pressedEntity in pressedFilter) _world.DelEntity(pressedEntity);
                    }
                }
            }
        }

        private void OnMousePressedStarted(InputAction.CallbackContext obj)
        {
            if (_isPointerOverUI) return;
            
            var mousePosition = _mousePosition.ReadValue<Vector2>();
            var raycastPosition = RaycastGround(mousePosition);
            if (raycastPosition == null) return;

            var newEntity = _world.NewEntity();
            var pool = _world.GetPool<MousePressing>();

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
                    var positionInt = Vector3Int.RoundToInt(hit.point);
                    var gridPositionInt = new Vector2Int(positionInt.x, positionInt.z);
                    return gridPositionInt;
                }
            }

            return null;
        }
    }
}