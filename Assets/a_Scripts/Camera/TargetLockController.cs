using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetLockController : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public float maxLockDistance = 2f;
    [Range(0f, 180f)] public float maxLockAngle = 110f;
    public LayerMask occlusionMask = ~0;

    private Transform player;
    private Camera mainCam;

    private void Awake()
    {
        player = transform;
        mainCam = Camera.main;
    }

    private void Update()
    {
        //TODO:接入 InputSystem
        if (Input.GetMouseButtonDown(2))
        {
            var mgr = GetComponent<PlayerCameraManager>();
            if (mgr != null)
            {
                mgr.ToggleLock();
            }
        }
    }
    
    public List<Transform> GetCandidateTargets()
    {
        var list = new List<Transform>();
        var all = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (var go in all)
        {
            if (go == null)
            {
                continue;
            }
            var t = go.transform;
            float d = Vector3.Distance(mainCam.transform.position, t.position);
            if (d > maxLockDistance)
            {
                continue;
            }

            if (!IsInFrontOfCamera(t))
            {
                continue;
            }

            if (!IsVisible(t))
            {
                continue;
            }
            list.Add(t);
        }

        //排序，角度越小优先，再近的
        list = list.OrderBy(t =>
        {
            float angle = Vector3.Angle(mainCam.transform.forward, (t.position - mainCam.transform.position).normalized);
            float dist = Vector3.Distance(player.position, t.position);
            return angle * 1000f + dist;
        }).ToList();

        return list;
    }

    private bool IsInFrontOfCamera(Transform t)
    {
        Vector3 dir = (t.position - mainCam.transform.position).normalized;
        float angle = Vector3.Angle(mainCam.transform.forward, dir);
        return angle <= maxLockAngle;
    }

    private bool IsVisible(Transform t)
    {
        Vector3 camPos = mainCam.transform.position;
        Vector3 targetPos = t.position + Vector3.up * 1.2f;
        Vector3 dir = targetPos - camPos;
        if (Physics.Raycast(camPos, dir.normalized, out RaycastHit hit, dir.magnitude, occlusionMask))
        {
            if (hit.transform == t || hit.transform.IsChildOf(t))
            {
                return true;
            }
            return false;
        }
        return true;
    }
}
