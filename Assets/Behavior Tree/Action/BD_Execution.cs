using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_PlayExecution : Action
{
    public SharedTransform player;
    public Vector3 playerOffset = new Vector3(0, 0, -1f);

    private CharacterStats enemyStats;
    private CharacterStats playerStats;
    private Animator animator;
    private bool started;

    public string executionAnimName = "Execution";
    public string fallbackState = "Idle";

    public override void OnAwake()
    {
        enemyStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();

        if (player != null && player.Value != null)
        {
            playerStats = player.Value.GetComponent<CharacterStats>();
        }
    }

    public override void OnStart()
    {
        started = true;
        animator.CrossFadeInFixedTime(executionAnimName, 0.1f);

        if (player != null && player.Value != null)
        {
            Vector3 targetPos = transform.position + transform.forward * playerOffset.z;
            player.Value.position = targetPos;
            player.Value.rotation = Quaternion.LookRotation(-transform.forward);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!started)
        {
            if (playerStats != null)
                playerStats.shouldPlayExecutionAnim = false;

            return TaskStatus.Failure;
        }

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(executionAnimName) && state.normalizedTime >= 1f)
        {
            animator.CrossFadeInFixedTime(fallbackState, 0.1f, 0);
            enemyStats.ResetPoise();
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        if (playerStats != null)
        {
            playerStats.shouldPlayExecutionAnim = false;
        }
    }
}