using System;
using UnityEngine;

namespace TownBuilder.Setup
{
    [Serializable]
    public class PrefabSpawnData
    {
        public GameObject Prefab;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}