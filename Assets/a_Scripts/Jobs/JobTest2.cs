using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Burst; 
using Unity.Jobs;

public class JobTest2 : MonoBehaviour
{
    private void Start()
    {
        //Temp: 内存分配后必须在同一帧结束前释放，通常用于主线程的极短期操作
        //用途：主线程的临时计算（如数学中间结果）。快速数据转换（如 Mesh.vertices 转 NativeArray）
        
        //TempJob ：内存可跨多帧存在，但必须在 Job 完成后手动释放（通过 Dispose()）
        //Job System 中的临时数据传递。跨线程计算结果缓存。
        
        NativeArray<float> input = new NativeArray<float>(1000, Allocator.TempJob);
        NativeArray<float> output = new NativeArray<float>(1000, Allocator.TempJob);

        for (int i = 0; i < input.Length; i++)
        {
            input[i] = i;
        }

        var job = new ParallelSquareJob()
        {
            input = input,
            output = output,
            
        };
        
        JobHandle handle = job.Schedule(input.Length, 64);
        handle.Complete();
        
        Debug.Log("Output[5]: " + output[5]);
        
        input.Dispose();
        output.Dispose();

    }

    struct ParallelSquareJob : IJobParallelFor
    {
        public NativeArray<float> input;
        public NativeArray<float> output;
        public void Execute(int index)
        {
            output[index] = input[index] * input[index];
        }
    }
    
    

    
    
    
}
