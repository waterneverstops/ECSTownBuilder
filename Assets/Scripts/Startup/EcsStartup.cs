using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;
using TownBuilder.MonoComponents;
using TownBuilder.Setup;
using TownBuilder.Systems;
using TownBuilder.Systems.Camera;
using UnityEngine;

namespace TownBuilder.Startup
{
    internal sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private PrefabSetup _prefabSetup;
        [SerializeField] private PrefabFactory _prefabFactory;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private InputActions _inputActions;

        private void Awake()
        {
            _world = new EcsWorld();

            _inputActions = new InputActions();

            _systems = new EcsSystems(_world);
            _systems
                .Add(new PrefabSpawnSystem())
                .Add(new CameraSpawnSystem())
                .Add(new CameraInputSystem())
                .Add(new CameraMovementSystem())
                .Add(new CameraRotationSystem())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif

                .Inject(_inputActions,
                    _prefabSetup,
                    _prefabFactory)
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}