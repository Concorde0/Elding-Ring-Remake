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
        // 如果处决状态取消，返回 Success
        if (!stats.isExecution)
        {
            return TaskStatus.Success;
        }

        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(executionIdleAnim) && state.normalizedTime >= 0.95f)
        {
            stats.ResetPoise();
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;

    }
}