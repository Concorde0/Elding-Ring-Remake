using UnityEngine;

[ExecuteAlways]
public class EnemyBarFollower : MonoBehaviour
{
    [Header("Target (必填)")]
    public CharacterStats target;
    public Transform followTarget;
    public Vector3 worldOffset = new(0f, 1.8f, 0f);

    [Header("显示控制")]
    public bool faceCamera = true;
    public bool hideWhenDead = true;
    public float minShowDistance = 0f;

    private Canvas parentCanvas;
    private Camera canvasCamera;
    private Camera mainCam;
    private RectTransform thisRect;

    private void Awake()
    {
        thisRect = transform as RectTransform;
        mainCam = Camera.main;
        ResolveCanvas();
        ResolveFollowTarget();
    }

    private void OnValidate()
    {
        ResolveCanvas();
        ResolveFollowTarget();
    }

    private void LateUpdate()
    {
        if (!IsValidTarget())
        {
            return;
        }

        if (parentCanvas == null || parentCanvas.renderMode == RenderMode.WorldSpace)
        {
            UpdateWorldSpace();
        }

        else
        {
            UpdateScreenSpace();
        }
            
    }

    private bool IsValidTarget()
    {
        if (target == null || (hideWhenDead && target.CurrentHealth <= 0))
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return false;
        }

        if (followTarget == null)
        {
            ResolveFollowTarget();
        }
        return followTarget != null;
    }

    private void ResolveCanvas()
    {
        parentCanvas = GetComponentInParent<Canvas>();
        canvasCamera = parentCanvas?.renderMode == RenderMode.ScreenSpaceCamera ? parentCanvas.worldCamera ?? Camera.main : null;
    }

    private void ResolveFollowTarget()
    {
        if (followTarget != null || target == null)
        {
            return;
        }

        followTarget = target.weaponSlot ?? target.transform.Find("UI_FollowPoint") ?? CreateFollowPoint(target.transform);
    }

    private Transform CreateFollowPoint(Transform parent)
    {
        var go = new GameObject("UI_FollowPoint");
        go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.up * 1.7f;
        return go.transform;
    }

    private void UpdateWorldSpace()
    {
        transform.position = followTarget.position + worldOffset;

        if (faceCamera && mainCam != null)
        {
            var dir = transform.position - mainCam.transform.position;
            if (dir.sqrMagnitude > 0.00001f)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        UpdateVisibility();
    }

    private void UpdateScreenSpace()
    {
        if (thisRect == null || parentCanvas == null || mainCam == null) return;

        var cam = canvasCamera ?? mainCam;
        var worldPos = followTarget.position + worldOffset;
        var screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.GetComponent<RectTransform>(), screenPoint, canvasCamera, out var localPoint))
        {
            thisRect.anchoredPosition = localPoint;
        }

        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (minShowDistance <= 0f || mainCam == null) return;

        float d = Vector3.Distance(mainCam.transform.position, followTarget.position);
        bool show = d >= minShowDistance;
        if (gameObject.activeSelf != show) gameObject.SetActive(show);
    }
}
