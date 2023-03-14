using Leopotam.EcsLite;
using TownBuilder.Components;
using TownBuilder.MonoComponents.MonoLinks.Base;

namespace TownBuilder.MonoComponents.MonoLinks
{
    public class MovableMonoLink : MonoLink<Movable>
    {
        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var pool = world.GetPool<Movable>();

                if (pool.Has(entity)) return;

                ref var component = ref pool.Add(entity);
                component.Position = transform.position;
            }
        }
    }
}