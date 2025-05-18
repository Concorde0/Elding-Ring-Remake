using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobTest3 : MonoBehaviour
{
    private void Start()
    {
        NativeList<int> list = new NativeList<int>(Allocator.TempJob);

        var job = new ListAddJob()
        {
            list = list,
        };
        JobHandle handle = job.Schedule();
        handle.Complete();
        
        list.Dispose();
        
        
    }

    struct ListAddJob : IJob
    {
        public NativeList<int> list;
        public void Execute()
        {
            for (int i = 0; i < list.Length; i++)
            {
                list.Add(i);
            }    
        }
    }
}
