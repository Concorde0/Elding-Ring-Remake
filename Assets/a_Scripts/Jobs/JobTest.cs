using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine;

public class JobTest : MonoBehaviour
{
    void Start()
    {
        var result = new NativeArray<float>(1, Allocator.TempJob);

        var job = new AddJob { a = 5, b = 3, result = result };
        JobHandle handle = job.Schedule();
        handle.Complete();

        Debug.Log("Job Result: " + result[0]); // 应该输出 8
        result.Dispose();
    }

    [BurstCompile]
    struct AddJob : IJob
    {
        public float a, b;
        public NativeArray<float> result;

        public void Execute()
        {
            result[0] = a + b;
        }
    }
}