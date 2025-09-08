using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_PlayStagger : Action
{
   
    private Animator animator;
    private bool started;

    public string staggerAnimName = "Hurt1"; // Animator 状态名

    public override void OnAwake()
    {
        animator   = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        started = true;
        animator.CrossFadeInFixedTime(staggerAnimName, 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        if (!started) return TaskStatus.Failure;

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(staggerAnimName) && state.normalizedTime >= 1f)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}