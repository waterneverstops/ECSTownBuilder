using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Links;
using TownBuilder.Components.Structures;
using TownBuilder.SO;

namespace TownBuilder.Systems.Structures
{
    public class RefreshHouseViewSystem : IEcsInitSystem, IEcsRunSystem
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

            var houseFilter = world.Filter<House>().Inc<RefreshHouseView>().End();

            var levelPool = world.GetPool<StructureLevel>();
            var viewPool = world.GetPool<ViewSwapperLink>();

            foreach (var houseEntity in houseFilter)
            {
                var level = levelPool.Get(houseEntity).Level;
                var viewSwapper = viewPool.Get(houseEntity).Value;
                
                viewSwapper.SwapView(_houseConfig.LevelDescriptions[level].SmallView);
            }
        }
    }
}