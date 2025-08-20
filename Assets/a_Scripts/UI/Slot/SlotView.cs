using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject highlight;

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
}