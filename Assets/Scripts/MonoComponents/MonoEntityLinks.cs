using System.Collections.Generic;
using TownBuilder.MonoComponents.MonoLinks.Base;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class MonoEntityLinks : MonoBehaviour
    {
        [SerializeField] private List<MonoEntity> _monoEntities;

        public IReadOnlyList<MonoEntity> MonoEntities => _monoEntities;
    }
}