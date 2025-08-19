using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraInputBlocker : MonoBehaviour
{
    private CinemachineInputProvider _provider;

    void Awake()
    {
        _provider = GetComponent<CinemachineInputProvider>();
        if (_provider == null)
            Debug.LogError("CameraInputBlocker: Missing InputSystemInputProvider.");
    }

    void Update()
    {
        if (_provider == null) return;

        bool isOverUI = EventSystem.current != null &&
                        EventSystem.current.IsPointerOverGameObject();

        // 启用或禁用输入
        _provider.enabled = !isOverUI;

    }
}