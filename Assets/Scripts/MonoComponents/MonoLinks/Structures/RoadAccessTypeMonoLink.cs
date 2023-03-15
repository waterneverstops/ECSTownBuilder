using Leopotam.EcsLite;
using TownBuilder.Components.Structures;
using TownBuilder.MonoComponents.MonoLinks.Base;

namespace TownBuilder.MonoComponents.MonoLinks.Structures
{
    public class RoadAccessTypeMonoLink : MonoLink<RoadAccessType>
    {
        public bool IsTwoCellRadius;

        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var pool = world.GetPool<RoadAccessType>();

                if (pool.Has(entity)) return;

                ref var component = ref pool.Add(entity);
                component.IsTwoCellRadius = IsTwoCellRadius;
            }
        }
    }
}