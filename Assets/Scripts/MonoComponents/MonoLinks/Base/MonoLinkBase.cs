using Leopotam.EcsLite;
using UnityEngine;

namespace TownBuilder.MonoComponents.MonoLinks.Base
{
    [RequireComponent(typeof(MonoEntity))]
    public abstract class MonoLinkBase : MonoBehaviour
    {
        public abstract void Make(EcsPackedEntityWithWorld packedEntityWithWorld);
    }
}