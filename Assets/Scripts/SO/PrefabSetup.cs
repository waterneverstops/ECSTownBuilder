using System.Collections.Generic;
using TownBuilder.SO.RoadSetup;
using UnityEngine;

namespace TownBuilder.SO
{
    [CreateAssetMenu(fileName = "PrefabSetup", menuName = "ScriptableObjects/PrefabSetup", order = 0)]
    public class PrefabSetup : ScriptableObject
    {
        public PrefabSpawnData CameraPrefab;
        public PrefabSpawnData DebugDrawerPrefab;

        public GameObject BaseHousePrefab;
        public GameObject GhostHousePrefab;
        
        public GameObject HunterHutPrefab;
        public GameObject MarketPrefab;

        public RoadPrefabSetup RoadPrefabSetup;
    }
}