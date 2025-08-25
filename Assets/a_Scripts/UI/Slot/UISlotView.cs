using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UISlotView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject highlight;

    // 注意：左/右点击会传入屏幕坐标（方便弹窗）
    public event Action<Vector2> OnLeftClick;
    public event Action<Vector2> OnRightClick;
    public event Action OnHoverEnter;
    public event Action OnHoverExit;

    private void OnDestroy()
    {
        OnLeftClick = null;
        OnRightClick = null;
        OnHoverEnter = null;
        OnHoverExit = null;
    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
        iconImage.enabled = icon != null;
    }

    public void SetHighlight(bool active)
    {
        if (highlight != null)
            highlight.SetActive(active);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick?.Invoke(eventData.position);
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick?.Invoke(eventData.position);
    }

    public void OnPointerEnter(PointerEventData eventData) => OnHoverEnter?.Invoke();
    public void OnPointerExit(PointerEventData eventData) => OnHoverExit?.Invoke();
}