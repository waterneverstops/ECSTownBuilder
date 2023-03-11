using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class ViewSwapper : MonoBehaviour
    {
        [SerializeField] private GameObject _view;

        public void SwapView(ViewVariant viewVariant)
        {
            if (_view != null)
            {
                Destroy(_view);
            }

            var newView = Instantiate(viewVariant.Prefab, viewVariant.Position, viewVariant.Rotation);
            newView.transform.parent = transform;
            _view = newView;
        }
    }
}