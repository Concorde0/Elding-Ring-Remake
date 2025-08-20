using RPG.UI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SlotView))]
public class Slot : MonoBehaviour
{
    public UnityEvent<Slot> OnLeftClick = new UnityEvent<Slot>();
    public UnityEvent<Slot> OnRightClick = new UnityEvent<Slot>();

    private SlotView view;
    private ISlotData data;

    [SerializeField] private HoverClickable clickable;

    private void Awake()
    {
        view = GetComponent<SlotView>();

        if (clickable != null)
        {
            clickable.OnHoverEnter.AddListener(() => view.SetHighlight(true));
            clickable.OnHoverExit.AddListener(() => view.SetHighlight(false));
            clickable.OnLeftClick.AddListener(() => OnLeftClick.Invoke(this));
            clickable.OnRightClick.AddListener(() => OnRightClick.Invoke(this));
        }
    }

    public void SetData(ISlotData newData)
    {
        data = newData;
        view.SetIcon(data?.Icon);
    }

    public ISlotData GetData() => data;
}