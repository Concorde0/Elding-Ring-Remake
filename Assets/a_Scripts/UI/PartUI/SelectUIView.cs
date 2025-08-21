using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class SelectUIView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Button useButton;
        [SerializeField] private Button dropButton;
        [SerializeField] private Button closeButton;

        private void OnEnable()
        {
            useButton?.onClick.AddListener(OnUse);
            dropButton?.onClick.AddListener(OnDrop);
            closeButton?.onClick.AddListener(OnClose);
        }

        private void OnDisable()
        {
            useButton?.onClick.RemoveAllListeners();
            dropButton?.onClick.RemoveAllListeners();
            closeButton?.onClick.RemoveAllListeners();
        }

        public void SetPosition(Vector2 screenPos, Canvas canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPos);

            rectTransform.localPosition = localPos;
        }

        private void OnUse()
        {
            Debug.Log("使用物品");
            Destroy(gameObject);
        }

        private void OnDrop()
        {
            Debug.Log("丢弃物品");
            Destroy(gameObject);
        }

        private void OnClose()
        {
            Destroy(gameObject);
        }
    }
}