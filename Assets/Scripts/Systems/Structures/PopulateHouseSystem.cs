using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using TownBuilder.Components.Structures;

namespace TownBuilder.Systems.Structures
{
    public class PopulateHouseSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            var moveInFilter = world.Filter<MoveSettlerIn>().Inc<House>().Inc<RequestedSettlers>().Inc<AllocatedSettlers>().Inc<AwaitingSettlers>()
                .End();

            var moveInPool = world.GetPool<MoveSettlerIn>();
            var requestPool = world.GetPool<RequestedSettlers>();
            var allocatedPool = world.GetPool<AllocatedSettlers>();
            var awaitingPool = world.GetPool<AwaitingSettlers>();
            var housePool = world.GetPool<House>();

            foreach (var moveInEntity in moveInFilter)
            {
                ref var moveInComponent = ref moveInPool.Get(moveInEntity);
                moveInComponent.Amount--;
                if (moveInComponent.Amount == 0) moveInPool.Del(moveInEntity);

                ref var requestComponent = ref requestPool.Get(moveInEntity);
                requestComponent.Amount--;
                if (requestComponent.Amount == 0) requestPool.Del(moveInEntity);

                ref var allocatedComponent = ref allocatedPool.Get(moveInEntity);
                allocatedComponent.Amount--;
                if (allocatedComponent.Amount == 0) allocatedPool.Del(moveInEntity);

                ref var awaitingComponent = ref awaitingPool.Get(moveInEntity);
                awaitingComponent.Amount--;
                if (awaitingComponent.Amount == 0) awaitingPool.Del(moveInEntity);
                
                ref var houseComponent = ref housePool.Get(moveInEntity);
                houseComponent.Population++;
            }
        }
    }
}