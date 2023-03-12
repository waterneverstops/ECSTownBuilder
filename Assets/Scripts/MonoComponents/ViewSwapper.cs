using TownBuilder.SO;
using UnityEngine;

namespace TownBuilder.MonoComponents
{
    public class ViewSwapper : MonoBehaviour
    {
        [SerializeField] private GameObject _view;
        [SerializeField] private Transform _viewParent;

        public void SwapView(ViewVariant viewVariant)
        {
            if (_view != null)
            {
                Destroy(_view);
            }

            var newView = Instantiate(viewVariant.Prefab, _viewParent);
            newView.transform.localPosition = viewVariant.Position;
            newView.transform.localRotation = viewVariant.Rotation;
            _view = newView;
        }
    }
}