using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraInputBlocker : MonoBehaviour
{
    private CinemachineInputProvider _provider;

    private void Awake()
    {
        _provider = GetComponent<CinemachineInputProvider>();
    }

    private void Update()
    {
        if (_provider == null) return;

        bool isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        _provider.enabled = !isOverUI;

    }
}