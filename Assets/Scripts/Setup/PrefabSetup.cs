using System.Collections.Generic;
using UnityEngine;

namespace TownBuilder.Setup
{
    [CreateAssetMenu(fileName = "SpawnPrefabSetup", menuName = "ScriptableObjects/SpawnPrefabSetup", order = 0)]
    public class PrefabSetup : ScriptableObject
    {
        public PrefabSpawnData CameraPrefab;
    }
}