using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_WaitForExecution : Action
{
    private CharacterStats stats;
    private Animator animator;

    public string executionIdleAnim = "Fell"; // 敌人待处决待机动画

    public override void OnAwake()
    {
        stats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        animator.CrossFadeInFixedTime(executionIdleAnim, 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        //TODO:动画播放完成后Failure
        if (!stats.isCritical) 
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}