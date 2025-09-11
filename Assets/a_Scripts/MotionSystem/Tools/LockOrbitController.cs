// using UnityEngine;
//
// public class LockOrbitController
// {
//     public Transform Target { get; private set; }
//
//     // 参数（可以按需暴露到外部）
//     public float OrbitDegreesPerSecond = 160f; // 角速度（度/秒）当 input.x == 1
//     public float RadialSpeed = 2.5f;           // 半径变化速度（单位/秒）当 input.y == 1
//     public float MinRadius = 0.7f;
//     public float MaxRadius = 6.0f;
//
//     // 内部状态
//     private float _radius = 2f;    // 当前半径
//     private float _angleRad = 0f;  // 当前角度（弧度）
//     private bool _initialized = false;
//     private float _fixedY = 0f;    // 保存 model 的 Y 以便保持高度（可改为地面高度查询）
//
//     // 获取最新计算出的世界位置（在 Update 后有效）
//     public Vector3 WorldPosition { get; private set; }
//
//     /// <summary>
//     /// 设置目标并用 modelPosition 初始化半径/角度/高度。
//     /// </summary>
//     public void SetTarget(Transform target, Vector3 modelPosition)
//     {
//         Target = target;
//         _initialized = false;
//         if (target != null)
//         {
//             Vector3 rel = modelPosition - Target.position;
//             rel.y = 0f;
//             _radius = Mathf.Clamp(rel.magnitude, MinRadius, MaxRadius);
//             _angleRad = Mathf.Atan2(rel.z, rel.x);
//             _fixedY = modelPosition.y;
//             WorldPosition = new Vector3(Target.position.x + Mathf.Cos(_angleRad) * _radius,
//                                         _fixedY,
//                                         Target.position.z + Mathf.Sin(_angleRad) * _radius);
//             _initialized = true;
//         }
//     }
//
//     public void ClearTarget()
//     {
//         Target = null;
//         _initialized = false;
//     }
//
//     /// <summary>
//     /// 根据 input 更新角度与半径，计算新 world position（XZ 平面），保持 Y 为最初固定高度。
//     /// input.x : 左右（-1..1） -> 角速度
//     /// input.y : 前后（-1..1） -> 径向变化（向前通常靠近目标）
//     /// </summary>
//     public void Update(Vector2 input, float deltaTime)
//     {
//         if (Target == null) return;
//
//         if (!_initialized)
//         {
//             // 如果没有被初始化，用当前 model 位置估计
//             SetTarget(Target, WorldPosition);
//         }
//
//         // 角度（弧度）
//         float angleDeltaDeg = input.x * OrbitDegreesPerSecond * deltaTime;
//         _angleRad += angleDeltaDeg * Mathf.Deg2Rad;
//
//         // 径向变化：我们约定 input.y > 0 -> 向前 -> 靠近目标（减小半径）
//         float radiusDelta = -input.y * RadialSpeed * deltaTime; // 负号表示前进减小半径
//         _radius = Mathf.Clamp(_radius + radiusDelta, MinRadius, MaxRadius);
//
//         // 计算世界坐标（保持 Y 不变）
//         float nx = Mathf.Cos(_angleRad) * _radius;
//         float nz = Mathf.Sin(_angleRad) * _radius;
//         WorldPosition = new Vector3(Target.position.x + nx, _fixedY, Target.position.z + nz);
//     }
// }
