using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlyAttack : MonoBehaviour
{
    public SkinnedMeshRenderer Fly;

    public void OpenTail()
    {
        Fly.enabled = true;
    }
    

    public void CloseTail()
    {
        Fly.enabled = false;
    }
}
