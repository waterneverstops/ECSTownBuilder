using Leopotam.EcsLite;
using TownBuilder.Components.Grid;
using TownBuilder.MonoComponents.MonoLinks.Base;

namespace TownBuilder.MonoComponents.MonoLinks.Building
{
    public class RoadMonoLink : MonoLink<Road>
    {
        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var pool = world.GetPool<Road>();

                if (pool.Has(entity)) return;

                ref var component = ref pool.Add(entity);
                component.Parent = entity;
            }
        }
    }
}