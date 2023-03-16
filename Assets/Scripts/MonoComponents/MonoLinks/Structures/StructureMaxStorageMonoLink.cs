using Leopotam.EcsLite;
using TownBuilder.Components.Structures;
using TownBuilder.MonoComponents.MonoLinks.Base;
using UnityEngine;

namespace TownBuilder.MonoComponents.MonoLinks.Structures
{
    public class StructureMaxStorageMonoLink : MonoLink<StructureMaxStorage>
    {
        [SerializeField] private int _maxFood;

        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            if (packedEntityWithWorld.Unpack(out var world, out var entity))
            {
                var pool = world.GetPool<StructureMaxStorage>();

                if (pool.Has(entity)) return;

                ref var component = ref pool.Add(entity);
                component.MaxFood = _maxFood;
            }
        }
    }
}