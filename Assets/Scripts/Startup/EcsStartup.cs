using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.MonoComponents;
using TownBuilder.SO;
using TownBuilder.Systems;
using TownBuilder.Systems.Building;
using TownBuilder.Systems.Camera;
using TownBuilder.Systems.DebugSystems;
using TownBuilder.Systems.RoadDisjointSetSystems;
using TownBuilder.Systems.Structures;
using TownBuilder.Systems.UGui;
using UnityEngine;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

namespace TownBuilder.Startup
{
    public sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private EcsUguiEmitter _uguiEmitter;

        [SerializeField] private LevelDescription _levelDescription;
        [SerializeField] private PrefabSetup _prefabSetup;
        [SerializeField] private PrefabFactory _prefabFactory;
        [SerializeField] private HouseConfig _houseConfig;

        private EcsWorld _world;
        private IEcsSystems _systems;

        private InputActions _inputActions;
        private LevelContext _levelContext;

        private void Awake()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();

            _levelContext = new LevelContext(_levelDescription);

            _world = new EcsWorld();
            
            _prefabFactory.Init(_world, _levelContext.MapGrid);

            _systems = new EcsSystems(_world);
            _systems
                // Before Destroy
                .Add(new FindRoadsToReMergeSystem())
                // Destroy
                .Add(new RefreshRoadNeighboursOnDestroySystem())
                .Add(new GridDestroySystem())
                .DelHere<Destroy>()
                // After Destroy
                .Add(new ReMergeRoadsSystem())
                .DelHere<ReMerge>()
                // Spawn
                .Add(new PrefabSpawnSystem())
                .DelHere<SpawnPrefab>()
                .Add(new GridPrefabSpawnSystem())
                .DelHere<SpawnPrefabGrid>()
                // Road Disjoint Set
                .Add(new RoadAddToDisjointSetSystem())
                .Add(new MergeRoadsSetSystem())
                // Grid And Building
                .Add(new GridInitializationSystem())
                .Add(new SingleBuilderSystem())
                .Add(new PathBuilderSystem())
                .Add(new PathGhostBuilderSystem())
                .Add(new AreaBuilderSystem())
                .Add(new AreaGhostBuilderSystem())
                .Add(new GhostCleanUpSystem())
                .Add(new NewRoadProcessingSystem())
                .DelHere<NewGridBuilding>()
                .Add(new RoadViewRefreshSystem())
                .DelHere<RefreshRoadModel>()
                .Add(new AreaDestroyerSystem())
                // Houses
                .Add(new HouseRequestSettlerSystem())
                .Add(new SpawnSettlerCountdownSystem())
                .Add(new SpawnSettlerSystem())
                .DelHere<SpawnSettlers>()
                // Input
                .Add(new CameraSpawnSystem())
                .Add(new CameraInputSystem())
                .Add(new CameraMovementSystem())
                .Add(new CameraRotationSystem())
                .Add(new MouseInputSystem())
                .Add(new ChooseBuildModeSystem())
                .Add(new ResetBuildModeSystem())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new RoadParentDrawDebugSystem())
#endif

                .Inject(_inputActions,
                    _prefabSetup,
                    _prefabFactory,
                    _houseConfig,
                    _levelContext)
                .InjectUgui(_uguiEmitter)
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