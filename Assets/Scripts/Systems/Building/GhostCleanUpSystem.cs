using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.Components.Building;
using TownBuilder.Components.Input;

namespace TownBuilder.Systems.Building
{
    public class GhostCleanUpSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var leftMouseReleasedFilter = world.Filter<LeftMouseReleased>().End();
            var rightMousePressedFilter = world.Filter<RightMousePressed>().End();
            if (leftMouseReleasedFilter.GetEntitiesCount() > 0 || rightMousePressedFilter.GetEntitiesCount() > 0)
            {
                var destroyPool = world.GetPool<Destroy>();
                var ghostFilter = world.Filter<GhostBuilding>().End();

                foreach (var ghostEntity in ghostFilter) destroyPool.Add(ghostEntity);
            }
        }
    }
}