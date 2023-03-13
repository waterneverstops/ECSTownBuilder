using Leopotam.EcsLite;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;

namespace TownBuilder.Systems.Building
{
    public class ResetBuildModeSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var pressedFilter = world.Filter<RightMousePressed>().End();

            foreach (var pressedEntity in pressedFilter)
            {
                var builderFilter = world.Filter<Builder>().End();
                foreach (var builderEntity in builderFilter) world.DelEntity(builderEntity);
            
                var destroyerFilter = world.Filter<Destroyer>().End();
                foreach (var destroyerEntity in destroyerFilter) world.DelEntity(destroyerEntity);                
            }
        }
    }
}