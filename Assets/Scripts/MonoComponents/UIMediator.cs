using TMPro;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class UIMediator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _populationText;
        
        public TMP_Text PopulationText => _populationText;
    }
}