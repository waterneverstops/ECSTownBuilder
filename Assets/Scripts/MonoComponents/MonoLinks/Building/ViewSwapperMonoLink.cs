using Leopotam.EcsLite;
using TownBuilder.Components.Links;
using TownBuilder.MonoComponents.MonoLinks.Base;

namespace TownBuilder.MonoComponents.MonoLinks.Building
{
    public class ViewSwapperMonoLink : MonoLink<GameObjectLink>
    {
        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var pool = world.GetPool<ViewSwapperLink>();

                if (pool.Has(entity)) return;

                ref var component = ref pool.Add(entity);
                component.Value = GetComponent<ViewSwapper>();
            }
        }
    }
}