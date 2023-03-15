using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;
using TownBuilder.SO;

namespace TownBuilder.Systems.Structures
{
    public class HouseRequestSettlerSystem : IEcsInitSystem, IEcsRunSystem
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

            var houseFilter = world.Filter<House>().Inc<BuildingLevel>().Exc<RequestedSettlers>().End();

            var housePool = world.GetPool<House>();
            var levelPool = world.GetPool<BuildingLevel>();
            var requestPool = world.GetPool<RequestedSettlers>();
            
            foreach (var houseEntity in houseFilter)
            {
                var population = housePool.Get(houseEntity).Population;
                var maxPopulation = _houseConfig.LevelDescriptions[levelPool.Get(houseEntity).Level].MaxCapacity;

                if (population < maxPopulation)
                {
                    ref var component = ref requestPool.Add(houseEntity);
                    component.Amount = maxPopulation - population;
                }
            }
        }
    }
}