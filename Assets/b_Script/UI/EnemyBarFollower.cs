using UnityEngine;

[ExecuteAlways]
public class EnemyBarFollower : MonoBehaviour
{
    [Header("Target (必填)")]
    public CharacterStats target;           // 敌人的 CharacterStats
    public Transform followTarget;          // 跟随点（为空时会尝试使用 target.weaponSlot 或创建一个在头顶的点）
    public Vector3 worldOffset = new Vector3(0f, 1.8f, 0f);

    [Header("显示控制")]
    public bool faceCamera = true;          // 在 WorldSpace 模式下是否朝向摄像机
    public bool hideWhenDead = true;        // 死亡时隐藏
    public float minShowDistance = 0f;      // 与摄像机距离小于该值则隐藏（0 表示永远显示）

    // 内部缓存
    Canvas parentCanvas;
    Camera canvasCamera;   // 当 Canvas 是 ScreenSpace-Camera 时使用；ScreenSpace-Overlay 则为 null
    Camera mainCam;
    RectTransform thisRect;

    void Awake()
    {
        thisRect = transform as RectTransform;
        mainCam = Camera.main;
        ResolveCanvas();
        EnsureFollowTarget();
    }

    void OnValidate()
    {
        EnsureFollowTarget();
        ResolveCanvas();
    }

    void ResolveCanvas()
    {
        // 自动寻找挂载在父物体上的 Canvas（如果你手动指定 parentCanvas，也会被覆盖）
        parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            if (parentCanvas.renderMode == RenderMode.ScreenSpaceCamera)
                canvasCamera = parentCanvas.worldCamera != null ? parentCanvas.worldCamera : Camera.main;
            else
                canvasCamera = null;
        }
    }

    void EnsureFollowTarget()
    {
        if (followTarget != null) return;
        if (target == null) return;

        if (target.weaponSlot != null) followTarget = target.weaponSlot;
        else
        {
            // 查找或创建一个默认点
            var t = target.transform.Find("UI_FollowPoint");
            if (t != null) followTarget = t;
            else
            {
                GameObject go = new GameObject("UI_FollowPoint");
                go.transform.SetParent(target.transform, false);
                go.transform.localPosition = Vector3.up * 1.7f;
                followTarget = go.transform;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }

        // 隐藏死了的敌人
        if (hideWhenDead && target.CurrentHealth <= 0)
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
            return;
        }

        if (followTarget == null) EnsureFollowTarget();
        if (followTarget == null) return;

        // 处理不同 Canvas 模式
        if (parentCanvas != null && parentCanvas.renderMode == RenderMode.WorldSpace)
        {
            // 直接在世界空间放置 UI
            transform.position = followTarget.position + worldOffset;

            if (faceCamera)
            {
                if (mainCam == null) mainCam = Camera.main;
                if (mainCam != null)
                {
                    Vector3 dir = transform.position - mainCam.transform.position;
                    if (dir.sqrMagnitude > 0.00001f)
                        transform.rotation = Quaternion.LookRotation(dir);
                }
            }

            // 距离隐藏（可选）
            if (minShowDistance > 0f && mainCam != null)
            {
                float d = Vector3.Distance(mainCam.transform.position, followTarget.position);
                bool show = d >= minShowDistance;
                if (gameObject.activeSelf != show) gameObject.SetActive(show);
            }
        }
        else
        {
            // Screen Space Canvas（Overlay 或 Camera）：把世界点转换为 Canvas 屏幕/本地坐标
            if (parentCanvas == null || thisRect == null)
            {
                // 没 Canvas，退回到简单的世界对齐（尽量保证能看到）
                transform.position = followTarget.position + worldOffset;
                return;
            }

            if (mainCam == null) mainCam = Camera.main;
            Camera camForWorldToScreen = (canvasCamera != null) ? canvasCamera : mainCam;

            Vector3 worldPos = followTarget.position + worldOffset;
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camForWorldToScreen, worldPos);

            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            Vector2 localPoint;
            bool ok = RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvasCamera, out localPoint);
            if (ok)
            {
                thisRect.anchoredPosition = localPoint;
            }

            // 对于 Screen Space，faceCamera 通常不需要（UI朝向由 Canvas 控制）
            if (minShowDistance > 0f && mainCam != null)
            {
                float d = Vector3.Distance(mainCam.transform.position, followTarget.position);
                bool show = d >= minShowDistance;
                if (gameObject.activeSelf != show) gameObject.SetActive(show);
            }
        }
    }
    
}
