using UnityEngine;
using Unity.Mathematics;

namespace RPG.MotionSystem
{
    /// <summary>
    /// 负责地面检测、斜坡、台阶、滑落、防穿地等。
    /// 非 MonoBehaviour，作为 PlayerMotor 的辅助类。
    /// </summary>
    public class PlayerGrounder
    {
        private readonly Transform _model;
        private readonly LayerMask _groundMask;
        private readonly float _rayLength = 1.5f;
        private readonly float _stepHeight = 0.4f;
        private readonly float _slopeLimit = 45f;

        public bool IsGrounded { get; private set; }
        public Vector3 GroundNormal { get; private set; }

        public PlayerGrounder(Transform model, LayerMask groundMask)
        {
            _model = model;
            _groundMask = groundMask;
        }

        public void ResolveGrounding()
        {
            Vector3 origin = _model.position + Vector3.up * 0.5f;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _rayLength, _groundMask))
            {
                IsGrounded = true;
                GroundNormal = hit.normal;

                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (slopeAngle <= _slopeLimit)
                {
                    // 贴地但不穿地
                    Vector3 targetPos = hit.point + Vector3.up * 0.02f;
                    _model.position = targetPos;
                }
            }
            else
            {
                IsGrounded = false;
            }
        }

        public void ResolveStepClimb()
        {
            Vector3 origin = _model.position + Vector3.up * 0.1f;
            Vector3 forward = _model.forward;

            if (Physics.Raycast(origin, forward, out RaycastHit hit, 0.5f, _groundMask))
            {
                Vector3 stepOrigin = _model.position + Vector3.up * _stepHeight;
                if (!Physics.Raycast(stepOrigin, forward, 0.5f, _groundMask))
                {
                    _model.position += Vector3.up * _stepHeight;
                }
            }
        }
    }
}
