using Leopotam.EcsLite;
using TownBuilder.MonoComponents.MonoLinks.Base;
using TownBuilder.Setup;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class PrefabFactory : MonoBehaviour
    {
        private EcsWorld _world;

        public void SetWorld(EcsWorld world)
        {
            _world = world;
        }

        public void Spawn(PrefabSpawnData spawnData)
        {
            var newObject = Instantiate(spawnData.Prefab, spawnData.Position, spawnData.Rotation);

            var links = newObject.GetComponent<MonoEntityLinks>();
            if (links != null)
            {
                foreach (var entityLink in links.MonoEntities) MakeEntity(entityLink);
                return;
            }

            var monoEntity = newObject.GetComponent<MonoEntity>();
            if (monoEntity == null)
                return;
            MakeEntity(monoEntity);
        }

        private void MakeEntity(MonoEntity monoEntity)
        {
            var entity = _world.NewEntity();
            var packedEntityWithWorld = _world.PackEntityWithWorld(entity);
            monoEntity.Make(packedEntityWithWorld);
        }
    }
}