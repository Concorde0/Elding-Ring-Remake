using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyGrounder : MonoBehaviour
{
    [Header("设置")]
    public LayerMask groundMask = -1;
    public float rayLength = 1.5f;
    public float stepHeight = 0.4f;
    public float slopeLimit = 45f;
    public float groundOffset = 0.02f;
    public float groundingSmooth = 10f;

    private Transform _model;
    private bool _isGrounded;

    private void Awake()
    {
        _model = transform;
    }

    private void FixedUpdate()
    {
        ResolveGrounding();
        ResolveStepClimb();
    }

    private void ResolveGrounding()
    {
        Vector3 origin = _model.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundMask))
        {
            _isGrounded = true;

            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle <= slopeLimit)
            {
                Vector3 targetPos = hit.point + Vector3.up * groundOffset;
                _model.position = Vector3.Lerp(_model.position, targetPos, Time.fixedDeltaTime * groundingSmooth);
            }
        }
        else
        {
            _isGrounded = false;
        }
    }

    private void ResolveStepClimb()
    {
        Vector3 origin = _model.position + Vector3.up * 0.1f;
        Vector3 forward = _model.forward;

        if (Physics.Raycast(origin, forward, out RaycastHit hit, 0.5f, groundMask))
        {
            Vector3 stepOrigin = _model.position + Vector3.up * stepHeight;
            if (!Physics.Raycast(stepOrigin, forward, 0.5f, groundMask))
            {
                Vector3 targetPos = _model.position + Vector3.up * stepHeight;
                _model.position = Vector3.Lerp(_model.position, targetPos, Time.fixedDeltaTime * groundingSmooth);
            }
        }
    }
}
