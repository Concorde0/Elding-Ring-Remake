using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_PlayExecution : Action
{
    private CharacterStats enemyStats;
    private Animator animator;
    private bool started;

    public string executionAnimName = "Execution"; // 处决动画名

    public override void OnAwake()
    {
        enemyStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        started = true;
        animator.CrossFadeInFixedTime(executionAnimName, 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        if (!started) return TaskStatus.Failure;

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(executionAnimName) && state.normalizedTime >= 1f)
        {
            enemyStats.ResetPoise();
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}