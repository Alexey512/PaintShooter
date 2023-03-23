using UnityEngine;
using UnityEngine.Events;

namespace Interactions
{
    [RequireComponent(typeof(Collider2D))]
    public class ClickInteraction: MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnClick;

        private void OnMouseDown()
        {
            OnClick?.Invoke();
        }
    }
}
