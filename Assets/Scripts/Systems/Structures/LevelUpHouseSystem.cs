using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.SO;

namespace TownBuilder.Systems.Structures
{
    public class LevelUpHouseSystem : IEcsInitSystem, IEcsRunSystem
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

            var houseFilter = world.Filter<House>().Inc<StructureLevel>().Inc<RoadAccess>().End();

            var housePool = world.GetPool<House>();
            var levelPool = world.GetPool<StructureLevel>();
            var refreshPool = world.GetPool<RefreshHouseView>();

            foreach (var houseEntity in houseFilter)
            {
                var population = housePool.Get(houseEntity).Population;
                ref var levelComponent = ref levelPool.Get(houseEntity);
                var maxPopulation = _houseConfig.LevelDescriptions[levelComponent.Level].MaxCapacity;

                if (population >= maxPopulation)
                {
                    if (levelComponent.Level < _houseConfig.LevelDescriptions.Count - 1)
                    {
                        levelComponent.Level++;
                        refreshPool.Add(houseEntity);
                    }
                }
            }
        }
    }
}