using System.Collections.Generic;
using TownBuilder.SO.RoadSetup;
using UnityEngine;

namespace TownBuilder.SO
{
    [CreateAssetMenu(fileName = "PrefabSetup", menuName = "ScriptableObjects/PrefabSetup", order = 0)]
    public class PrefabSetup : ScriptableObject
    {
        public List<PrefabSpawnData> SpawnOnInit;
        
        [Header("Characters")]
        public GameObject SettlerCharacterPrefab;
        public GameObject ExileCharacterPrefab;
        public GameObject HunterCharacterPrefab;

        [Header("House")]
        public GameObject BaseHousePrefab;
        public GameObject GhostHousePrefab;
        
        [Header("Structures")]
        public GameObject HunterHutPrefab;
        public GameObject MarketPrefab;
        public GameObject FoodSource;

        [Header("Roads")]
        public RoadPrefabSetup RoadPrefabSetup;
    }
}