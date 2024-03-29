using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.Unity.Ugui;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Characters;
using TownBuilder.Components.DisjointSet;
using TownBuilder.Components.Structures;
using TownBuilder.Context;
using TownBuilder.MonoComponents;
using TownBuilder.SO;
using TownBuilder.Systems;
using TownBuilder.Systems.Building;
using TownBuilder.Systems.Camera;
using TownBuilder.Systems.Characters;
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
        [SerializeField] private UIMediator _uiMediator;

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
            
            _prefabFactory.Init(_world, _levelContext.MapGrid, _prefabSetup);

            _systems = new EcsSystems(_world);
            _systems
                // Before Destroy
                .Add(new FindRoadsToReMergeSystem())
                .Add(new RefreshRoadAccessOnDestroySystem())
                .Add(new TriggerRefreshRoadAccessSystem())
                .DelHere<TriggerRefreshRoadAccess>()
                // Destroy
                .Add(new RefreshRoadNeighboursOnDestroySystem())
                .Add(new GridDestroySystem())
                .Add(new GameObjectDestroySystem())
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
                .DelHere<Merge>()
                // Grid And Building
                .Add(new GridInitializationSystem())
                .Add(new SingleBuilderSystem())
                .Add(new PathBuilderSystem())
                .Add(new PathGhostBuilderSystem())
                .Add(new AreaBuilderSystem())
                .Add(new AreaGhostBuilderSystem())
                .Add(new GhostCleanUpSystem())
                .Add(new NewRoadViewProcessingSystem())
                .Add(new NewRoadAccessProcessingSystem())
                .Add(new NewStructureAccessProcessingSystem())
                .DelHere<NewGridBuilding>()
                .Add(new RoadViewRefreshSystem())
                .DelHere<RefreshRoadModel>()
                .Add(new AreaDestroyerSystem())
                // Structures
                .Add(new SpawnFoodSourceSystem())
                .Add(new RefreshRoadAccessSystem())
                .DelHere<RefreshRoadAccess>()
                .Add(new RefreshWorkforceCountdownSystem())
                .Add(new RefreshWorkforceSystem())
                .DelHere<RefreshWorkforce>()
                .Add(new HunterWorkCountdownSystem())
                .Add(new HunterWorkSystem())
                .DelHere<HunterWork>()
                .Add(new MarketWorkCountdownSystem())
                .Add(new MarketWorkSystem())
                .DelHere<MarketWork>()
                .Add(new MarketSendCourierSystem())
                .DelHere<MarketSendCourier>()
                .Add(new MarketSendTraderSystem())
                .DelHere<MarketSendTrader>()
                // Houses
                .Add(new HouseRequestSettlerSystem())
                .Add(new SpawnSettlerCountdownSystem())
                .Add(new SpawnSettlerSystem())
                .DelHere<SpawnSettlers>()
                .Add(new PopulateHouseSystem())
                .Add(new SpawnExileCountdownSystem())
                .Add(new SpawnExileSystem())
                .DelHere<SpawnExiles>()
                .Add(new CleanUpExilesSystem())
                .Add(new LevelUpHouseSystem())
                .Add(new LevelDownHouseSystem())
                .Add(new RefreshHouseViewSystem())
                .DelHere<RefreshHouseView>()
                .Add(new FoodConsumptionCountdownSystem())
                .Add(new FoodConsumptionSystem())
                .DelHere<FoodConsumption>()
                // Character
                .Add(new GenerateSettlerPathSystem())
                .Add(new GenerateExilePathSystem())
                .Add(new GenerateHunterPathToFoodSystem())
                .Add(new GenerateHunterPathToHutSystem())
                .Add(new GenerateCourierPathToHunterSystem())
                .Add(new GenerateCourierPathToMarketSystem())
                .Add(new GenerateTraderWanderPathSystem())
                .Add(new GenerateTraderPathToMarketSystem())
                .Add(new FollowPathSystem())
                .DelHere<WanderStep>()
                .Add(new FollowWanderPathSystem())
                .Add(new LookAtMoveDirectionSystem())
                .Add(new MoveGameObjectSystem())
                .DelHere<Velocity>()
                .Add(new SettlerEnterSystem())
                .Add(new GatherHunterFoodSystem())
                .Add(new GatherCourierFoodSystem())
                .Add(new GatherTraderFoodSystem())
                .Add(new SellTraderFoodSystem())
                // Input
                .Add(new CameraInputSystem())
                .Add(new CameraMovementSystem())
                .Add(new CameraRotationSystem())
                .Add(new MouseInputSystem())
                .Add(new ChooseBuildModeSystem())
                .Add(new ResetBuildModeSystem())
                // UI
                .Add(new UpdatePopulationText())

#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
                .Add(new RoadParentDrawDebugSystem())
                .Add(new StructureRoadAccessDrawDebugSystem())
#endif

                .Inject(_inputActions,
                    _prefabSetup,
                    _prefabFactory,
                    _houseConfig,
                    _uiMediator,
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