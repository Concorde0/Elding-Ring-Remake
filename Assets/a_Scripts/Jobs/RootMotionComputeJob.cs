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
            float4x4 previous = localToWorldPrevious[index];
            float4x4 current = localToWorldCurrent[index];

            // 计算位置差
            deltaPositions[index] = current.c3.xyz - previous.c3.xyz;

            // 计算旋转差
            quaternion prevRot = new quaternion(previous);
            quaternion currRot = new quaternion(current);
            deltaRotations[index] = math.mul(math.inverse(prevRot), currRot);
        }
    } 
}
