using TMPro;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PickupUI : MonoBehaviour
{
    public static PickupUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private RectTransform promptPanel;

    private Camera _mainCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _mainCamera = Camera.main;
            promptPanel.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ShowPrompt(UnityEngine.Vector3 worldPosition, string itemId)
    {
        if (!_mainCamera) return;
        
        UnityEngine.Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPosition);
        if (screenPos.z < 0)
        {
            promptPanel.gameObject.SetActive(false);
            return;
        }

        promptPanel.gameObject.SetActive(true);
        promptPanel.position = screenPos;
    }
    public void HidePrompt()
    {
        promptPanel.gameObject.SetActive(false);
    }
}