using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using UnityEngine;


namespace JobSystem
{
/// <summary>
/// 非 MonoBehaviour 类，负责收集 Root 变换、调度 RootMotionComputeJob，并由外部统一管理生命周期
/// </summary>
public class RootMotionJobHandler
{
    private NativeArray<float4x4> prevTransforms;
    private NativeArray<float4x4> currTransforms;
    private NativeArray<float3> deltaPositions;
    private NativeArray<quaternion> deltaRotations;
    private int count;

    public JobHandle LastHandle { get; private set; }

    /// <summary>
    /// 初始化，必须传入需要同时计算的实例数量
    /// </summary>
    public RootMotionJobHandler(int instanceCount)
    {
        count = instanceCount;
        prevTransforms  = new NativeArray<float4x4>(count, Allocator.Persistent);
        currTransforms  = new NativeArray<float4x4>(count, Allocator.Persistent);
        deltaPositions  = new NativeArray<float3>(count, Allocator.Persistent);
        deltaRotations  = new NativeArray<quaternion>(count, Allocator.Persistent);
    }

    /// <summary>
    /// 记录每个实例的上一帧根节点矩阵
    /// </summary>
    /// <param name="index">实例索引</param>
    /// <param name="transform">该实例的根节点 Transform</param>
    public void RecordPrevious(int index, Transform transform)
    {
        prevTransforms[index] = float4x4.TRS(
            transform.position,
            transform.rotation,
            new float3(1,1,1)
        );
    }

    /// <summary>
    /// 在更新完 PlayableGraph 后，记录当前帧矩阵并调度 Job
    /// </summary>
    /// <param name="index">实例索引</param>
    /// <param name="transform">该实例的根节点 Transform</param>
    public void RecordAndSchedule(int index, Transform transform)
    {
        currTransforms[index] = float4x4.TRS(transform.position, transform.rotation, new float3(1,1,1));

        // 如果是最后一个实例，调度整个 Job
        if (index == count - 1)
        {
            var job = new RootMotionComputeJob
            {
                localToWorldPrevious = prevTransforms,
                localToWorldCurrent  = currTransforms,
                deltaPositions       = deltaPositions,
                deltaRotations       = deltaRotations
            };

            LastHandle = job.Schedule(count, 1);
        }
    }

    /// <summary>
    /// 等待 Job 完成，并获取结果
    /// </summary>
    public void CompleteAndApply(System.Action<int, float3, quaternion> applyCallback)
    {
        LastHandle.Complete();

        for (int i = 0; i < count; i++)
        {
            applyCallback(i, deltaPositions[i], deltaRotations[i]);
        }
    }

    /// <summary>
    /// 释放所有 NativeArray
    /// </summary>
    public void Dispose()
    {
        if (prevTransforms.IsCreated) prevTransforms.Dispose();
        if (currTransforms.IsCreated) currTransforms.Dispose();
        if (deltaPositions.IsCreated) deltaPositions.Dispose();
        if (deltaRotations.IsCreated) deltaRotations.Dispose();
    }
    }
}

