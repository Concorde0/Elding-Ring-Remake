using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailAttack : MonoBehaviour
{
    public GameObject tail;
    public SkinnedMeshRenderer Fly;

    public void OpenTail()
    {
        tail.SetActive(true);
    }

    public void CloseTail()
    {
        tail.SetActive(false);
    }
    
    public void OpenFly()
    {
        Fly.enabled = true;
    }

    public void CloseFly()
    {
        Fly.enabled = false;
    }
}
