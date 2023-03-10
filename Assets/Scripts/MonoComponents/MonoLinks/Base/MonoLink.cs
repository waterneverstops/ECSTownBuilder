using Leopotam.EcsLite;

namespace TownBuilder.MonoComponents.MonoLinks.Base
{
    public class MonoLink<T> : MonoLinkBase where T : struct
    {
        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var pool = world.GetPool<T>();

                if (pool.Has(entity)) return;

                pool.Add(entity);
            }
        }
    }
}