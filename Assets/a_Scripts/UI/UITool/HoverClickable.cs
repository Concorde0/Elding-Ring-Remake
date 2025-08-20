using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace RPG.UI
{
    public class HoverClickable : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("Highlight Target")]
        [SerializeField] private GameObject highlightObject; // 高亮的 Image 

        [Header("Events")]
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;
        public UnityEvent OnLeftClick;
        public UnityEvent OnRightClick;

        private void Awake()
        {
            if (highlightObject != null)
                highlightObject.SetActive(false); // 初始隐藏高亮
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (highlightObject != null)
                highlightObject.SetActive(true);

            OnHoverEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (highlightObject != null)
                highlightObject.SetActive(false);

            OnHoverExit?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                OnLeftClick?.Invoke();
            else if (eventData.button == PointerEventData.InputButton.Right)
                OnRightClick?.Invoke();
        }
    }
}