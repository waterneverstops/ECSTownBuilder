using Leopotam.EcsLite;
using TownBuilder.Context;
using TownBuilder.Context.LevelMapGrid;
using TownBuilder.MonoComponents.MonoLinks.Base;
using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class PrefabFactory : MonoBehaviour
    {
        private const string DecorLayerName = "Decor";
        private const string GridObjectsParentName = "Grid";

        private readonly Collider[] _results = new Collider[10];
        
        private EcsWorld _world;
        private MapGrid _mapGrid;

        private Transform _gridParentTransform;

        private LayerMask _decorMask;

        public void Init(EcsWorld world, MapGrid mapGrid)
        {
            _world = world;
            _mapGrid = mapGrid;

            _gridParentTransform = new GameObject(GridObjectsParentName).transform;

            _decorMask = LayerMask.GetMask(DecorLayerName);
        }

        public void Spawn(PrefabSpawnData spawnData)
        {
            var newObject = Instantiate(spawnData.Prefab, spawnData.Position, spawnData.Rotation);

            var links = newObject.GetComponent<MonoEntityLinks>();
            if (links != null)
            {
                foreach (var entityLink in links.MonoEntities) MakeNewEntity(entityLink);
                return;
            }

            var monoEntity = newObject.GetComponent<MonoEntity>();
            if (monoEntity == null)
                return;
            MakeNewEntity(monoEntity);
        }

        public void SpawnOnGrid(GameObject prefab, Vector2Int position)
        {
            var worldPosition = new Vector3Int(position.x, 0, position.y);

            ClearDecor(worldPosition);

            var newObject = Instantiate(prefab, worldPosition, Quaternion.identity);
            newObject.transform.parent = _gridParentTransform;

            var packedEntityWithWorld = _mapGrid[position];

            var monoEntity = newObject.GetComponent<MonoEntity>();

            monoEntity.Make(packedEntityWithWorld);
        }

        private void MakeNewEntity(MonoEntity monoEntity)
        {
            var entity = _world.NewEntity();
            var packedEntityWithWorld = _world.PackEntityWithWorld(entity);
            monoEntity.Make(packedEntityWithWorld);
        }

        private void ClearDecor(Vector3Int position)
        {
            var hits = Physics.OverlapBoxNonAlloc(position + Vector3.one * 0.5f, Vector3.one * 0.5f, _results, Quaternion.identity, _decorMask); 
            if (hits > 0)
            {
                for (var i = 0; i < hits; i++)
                {
                    var hit = _results[i];
                    hit.transform.gameObject.SetActive(false);
                }
            }
        }
    }
}