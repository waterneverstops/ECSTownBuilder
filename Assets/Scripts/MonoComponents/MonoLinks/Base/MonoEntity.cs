using Leopotam.EcsLite;

namespace TownBuilder.MonoComponents.MonoLinks.Base
{
    public class MonoEntity : MonoLinkBase
    {
        private EcsPackedEntityWithWorld _packedEntityWithWorld;

        private MonoLinkBase[] _monoLinks;

        public MonoLink<T> Get<T>() where T : struct
        {
            foreach (var link in _monoLinks)
                if (link is MonoLink<T> monoLink)
                    return monoLink;

            return null;
        }

        public override void Make(EcsPackedEntityWithWorld packedEntityWithWorld)
        {
            _packedEntityWithWorld = packedEntityWithWorld;

            _monoLinks = GetComponents<MonoLinkBase>();
            foreach (var monoLink in _monoLinks)
            {
                if (monoLink is MonoEntity) continue;
                monoLink.Make(packedEntityWithWorld);
            }
        }
    }
}