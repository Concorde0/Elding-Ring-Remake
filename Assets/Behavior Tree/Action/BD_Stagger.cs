using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_PlayStagger : Action
{
    private Animator animator;
    private bool started;

    public string staggerAnimName = "Hurt1"; // 受击动画状态名
    public string fallbackState = "Idle";    // 播放完回Idle

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        started = true;
        animator.CrossFadeInFixedTime(staggerAnimName, 0.1f);
        Debug.Log("!11111111111111111111111111111111111111111");
    }

    public override TaskStatus OnUpdate()
    {
        if (!started) return TaskStatus.Failure;

        var state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(staggerAnimName) && state.normalizedTime >= 1f)
        {
            animator.CrossFadeInFixedTime(fallbackState, 0.1f, 0);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}