using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.SO;

namespace TownBuilder.Systems.Structures
{
    public class LevelDownHouseSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<HouseConfig> _houseConfigInjection = default;

        private HouseConfig _houseConfig;

        public void Init(IEcsSystems systems)
        {
            _houseConfig = _houseConfigInjection.Value;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var houseFilter = world.Filter<House>().Inc<StructureLevel>().End();

            var housePool = world.GetPool<House>();
            var levelPool = world.GetPool<StructureLevel>();
            var refreshPool = world.GetPool<RefreshHouseView>();
            var storagePool = world.GetPool<StructureStorage>();

            foreach (var houseEntity in houseFilter)
            {
                ref var levelComponent = ref levelPool.Get(houseEntity);
                if (levelComponent.Level == 0) continue;

                var prevLevelDescription = _houseConfig.LevelDescriptions[levelComponent.Level - 1];

                var population = housePool.Get(houseEntity).Population;
                var prevMaxPopulation = prevLevelDescription.MaxCapacity;

                var food = storagePool.Get(houseEntity).Food;
                var prevNeedFood = prevLevelDescription.FoodToUpgrade;

                if (population < prevMaxPopulation || food < prevNeedFood)
                {
                    levelComponent.Level--;
                    refreshPool.Add(houseEntity);
                }
            }
        }
    }
}