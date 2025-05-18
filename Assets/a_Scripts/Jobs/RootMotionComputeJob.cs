using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace JobSystem
{
    [BurstCompile]
    public struct RootMotionComputeJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<float4x4> localToWorldPrevious;
        [ReadOnly] public NativeArray<float4x4> localToWorldCurrent;

        [WriteOnly] public NativeArray<float3> deltaPositions;
        [WriteOnly] public NativeArray<quaternion> deltaRotations;

        public void Execute(int index)
        {
            float4x4 prev = localToWorldPrevious[index];
            float4x4 curr = localToWorldCurrent[index];

            // 计算位置差
            deltaPositions[index] = curr.c3.xyz - prev.c3.xyz;

            // 计算旋转差
            quaternion prevRot = new quaternion(prev);
            quaternion currRot = new quaternion(curr);
            deltaRotations[index] = math.mul(math.inverse(prevRot), currRot);
        }
    } 
}
