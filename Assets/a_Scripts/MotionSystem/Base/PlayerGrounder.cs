using UnityEngine;
using Unity.Mathematics;

namespace RPG.MotionSystem
{
    // 负责地面检测、斜坡、台阶、滑落、防穿地
   
    public class PlayerGrounder
    {
        private Vector3 _lastPosition;
        private readonly Transform _model;
        private readonly LayerMask _groundMask;

        //地面/斜坡/台阶参数
        [SerializeField] private float _rayLength = 1.5f;              // 地面检测射线长度
        [SerializeField] private float _slopeLimit = 45f;              // 最大斜坡角度

        //多点射线参数
        [SerializeField] private float _footHeight = 0.5f;             // 射线起点上移高度
        [SerializeField] private float _footRadius = 0.25f;            // 脚底采样圈半径
        [SerializeField] private int   _sampleCount = 5;               // 采样点数量（含中心）
        [SerializeField] private float _groundSnapOffset = 0.02f;      // 贴地上浮皮肤宽度

        //台阶检测参数
        [SerializeField] private float _stepCheckRange = 0.5f;         // 前方检测距离
        [SerializeField] private float _stepCheckHeight = 0.4f;        // 台阶检测高度
        [SerializeField] private float _maxStepHeight = 0.4f;          // 最大可攀爬高度
        [SerializeField] private float _minStepThreshold = 0.06f;      // 最小高度阈值（去抖）

        //SphereCast参数
        [SerializeField] private bool  _useSphereCastFirst = true;     // 是否优先使用 SphereCast
        [SerializeField] private float _sphereRadiusMultiplier = 1.0f; // SphereCast 的半径系数（基于 footRadius）
        [SerializeField] private float _sphereCastStartHeight = 0.6f;  // SphereCast 起点上移高度
        [SerializeField] private float _sphereCastLength = 1.2f;       // SphereCast 向下长度
        
        [SerializeField] private float _slideSpeed = 3f;          // 斜坡滑落速度
        [SerializeField] private float _groundSnapSpeed = 10f;    // 贴地插值基准速度

        private bool IsGrounded { get; set; }
        public Vector3 GroundNormal { get; private set; }

        public PlayerGrounder(Transform model, LayerMask groundMask)
        {
            _model = model;
            _groundMask = groundMask;
        }
        
        //地面检测（SphereCast优先+多点射线回退）与斜坡限制、贴地
        public void Grounding()
        {
            //SphereCast
            if (_useSphereCastFirst && TrySphereGround(out RaycastHit sphereHit, out Vector3 sphereNormal))
            {
                ApplyGroundHit(sphereHit, sphereNormal);
                Debug.Log("SphereCast");
                return;
            }

            //若SphereCast未命中，则使用多点射线
            TryMultiRayGround(out bool anyHit, out RaycastHit bestHit, out Vector3 averagedNormal);

            if (anyHit)
            {
                Debug.Log("GroundHit");
                ApplyGroundHit(bestHit, averagedNormal);
            }
            else
            {
                IsGrounded = false;
                GroundNormal = Vector3.up;
            }
            
            if (Physics.Raycast(_model.position + Vector3.up * _footHeight, Vector3.down, out RaycastHit hit, _rayLength, _groundMask))
            {
                IsGrounded = true;
                GroundNormal = hit.normal;

                float slopeAngle = Vector3.Angle(GroundNormal, Vector3.up);

                if (slopeAngle <= _slopeLimit)
                {
                    //基于速度的平滑贴地
                    float velocity = ((_model.position - _lastPosition) / Time.deltaTime).magnitude;
                    float snapLerp = Mathf.Clamp01(velocity / 5f); // 速度越快，snap越快
                    Vector3 targetPos = new Vector3(_model.position.x, hit.point.y + _groundSnapOffset, _model.position.z);
                    _model.position = Vector3.Lerp(_model.position, targetPos, snapLerp * Time.deltaTime * _groundSnapSpeed);
                }
                else
                {
                    //斜坡超限后沿斜坡滑落
                    Vector3 slideDir = new Vector3(GroundNormal.x, -GroundNormal.y, GroundNormal.z);
                    slideDir = Vector3.ProjectOnPlane(slideDir, Vector3.up).normalized;
                    _model.position += slideDir * (_slideSpeed * Time.deltaTime);
                }
            }
            else
            {
                IsGrounded = false;
                GroundNormal = Vector3.up;
            }

            _lastPosition = _model.position;
        }

        //SphereCast检测地面命中
        private bool TrySphereGround(out RaycastHit hit, out Vector3 averagedNormal)
        {
            Vector3 origin = _model.position + Vector3.up * _sphereCastStartHeight;
            float radius = Mathf.Max(0.01f, _footRadius * _sphereRadiusMultiplier);
            averagedNormal = Vector3.up;

            if (Physics.SphereCast(origin, radius, Vector3.down, out hit, _sphereCastLength, _groundMask, QueryTriggerInteraction.Ignore))
            {
                averagedNormal = hit.normal.normalized;
                return true;
            }

            return false;
        }
        
        // 多点向下射线检测（中心+环形），返回是否命中、最佳命中点和平均法线
        private void TryMultiRayGround(out bool anyHit, out RaycastHit bestHit, out Vector3 averagedNormal)
        {
            anyHit = false;
            bestHit = default;
            averagedNormal = Vector3.up;

            Vector3 center = _model.position;
            Vector3 upOffset = Vector3.up * _footHeight;

            int count = Mathf.Max(1, _sampleCount);
            Vector3[] samples = new Vector3[count];

            samples[0] = center;
            if (count > 1)
            {
                for (int i = 1; i < count; i++)
                {
                    float t = (i - 1) / (float)(count - 1);
                    float angle = t * Mathf.PI * 2f;
                    Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                    samples[i] = center + dir * _footRadius;
                }
            }

            float bestDistance = float.MaxValue;
            Vector3 normalSum = Vector3.zero;
            int normalCount = 0;

            foreach (var t in samples)
            {
                Vector3 origin = t + upOffset;
                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _rayLength, _groundMask, QueryTriggerInteraction.Ignore))
                {
                    anyHit = true;

                    float dist = origin.y - hit.point.y;
                    if (dist < bestDistance)
                    {
                        bestDistance = dist;
                        bestHit = hit;
                    }

                    normalSum += hit.normal;
                    normalCount++;
                }
            }

            if (anyHit && normalCount > 0)
            {
                averagedNormal = (normalSum / normalCount).normalized;
            }
            
           
        }

        // 根据命中结果应用 Grounded、法线与贴地
        private void ApplyGroundHit(RaycastHit hit, Vector3 normal)
        {
            IsGrounded = true;
            GroundNormal = normal;

            float slopeAngle = Vector3.Angle(normal, Vector3.up);
            if (slopeAngle <= _slopeLimit)
            {
                Vector3 targetPos = new Vector3(
                    _model.position.x,
                    hit.point.y + _groundSnapOffset,
                    _model.position.z
                );

                _model.position = targetPos;
            }
            else
            {
                // 斜坡超限
            }
        }
        

    
        //台阶检测，基于高度差
        public void StepClimb()
        {
            if (!IsGrounded) return;

            Vector3 forward = _model.forward.normalized;
            
            Vector3 rayOrigin = _model.position + forward * _stepCheckRange + Vector3.up * _stepCheckHeight;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, _stepCheckHeight * 3f, _groundMask))
            {
                float heightDiff = hit.point.y - _model.position.y;

                // 忽略过小的高度差
                if (heightDiff < 0.06f)
                    return;
                
                if (heightDiff <= _maxStepHeight)
                {
                    //插值上台阶，优化卡顿
                    Vector3 targetPos = new Vector3(_model.position.x, hit.point.y + 0.02f, _model.position.z);
                    _model.position = Vector3.Lerp(_model.position, targetPos, 0.5f);
                }
            }
        }
    }
}