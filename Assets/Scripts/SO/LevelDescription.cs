using UnityEngine;

namespace TownBuilder.SO
{
    [CreateAssetMenu(fileName = "LevelDescription", menuName = "ScriptableObjects/LevelDescription", order = 0)]
    public class LevelDescription : ScriptableObject
    {
        [SerializeField] private Vector2Int _mapSize;

        public Vector2Int MapSize => _mapSize;
    }
}