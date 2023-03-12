using TownBuilder.SO.RoadSetup;
using UnityEngine;

namespace TownBuilder.SO
{
    [CreateAssetMenu(fileName = "PrefabSetup", menuName = "ScriptableObjects/PrefabSetup", order = 0)]
    public class PrefabSetup : ScriptableObject
    {
        public PrefabSpawnData CameraPrefab;

        public RoadPrefabSetup RoadPrefabSetup;
    }
}