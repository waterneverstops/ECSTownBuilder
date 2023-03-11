using UnityEngine;

namespace TownBuilder.SO
{
    [CreateAssetMenu(fileName = "SpawnPrefabSetup", menuName = "ScriptableObjects/SpawnPrefabSetup", order = 0)]
    public class PrefabSetup : ScriptableObject
    {
        public PrefabSpawnData CameraPrefab;
    }
}