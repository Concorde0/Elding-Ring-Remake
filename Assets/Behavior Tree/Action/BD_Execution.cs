using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_PlayExecution : Action
{
    public SharedTransform player; // 在行为树中绑定玩家
    public Vector3 playerOffset = new Vector3(0, 0, -1f); // 玩家相对敌人位置（默认前方1米）

    private CharacterStats enemyStats;
    private Animator animator;
    private bool started;

    public string executionAnimName = "Execution";
    public string fallbackState = "Idle";

    public override void OnAwake()
    {
        enemyStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        started = true;
        animator.CrossFadeInFixedTime(executionAnimName, 0.1f);

        // 固定玩家位置
        if (player != null && player.Value != null)
        {
            Vector3 targetPos = transform.position + transform.forward * playerOffset.z;
            player.Value.position = targetPos;
            player.Value.rotation = Quaternion.LookRotation(-transform.forward); // 玩家面向敌人
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!started) return TaskStatus.Failure;

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(executionAnimName) && state.normalizedTime >= 1f)
        {
            animator.CrossFadeInFixedTime(fallbackState, 0.1f, 0);
            enemyStats.ResetPoise();
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}