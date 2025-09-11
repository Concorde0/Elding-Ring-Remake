using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BD_Die : Action
{
    private Animator animator;
    private bool started;
    private bool destroyed;
    
    public string deathAnimName = "Die";
    
    public float exitNormalizedTime = 0.95f;
    
    public float destroyDelay = 0f;

    public override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        started = true;
        destroyed = false;
        if (animator)
        {
            animator.CrossFadeInFixedTime(deathAnimName, 0.1f);
        }
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.UpdateQuestProgress("Enemy", 1);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (!started) return TaskStatus.Failure;
        if (destroyed) return TaskStatus.Success;
        

        var state = animator.GetCurrentAnimatorStateInfo(0);
        
        if (state.IsName(deathAnimName) && state.normalizedTime >= exitNormalizedTime)
        {
            var nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (nav) nav.enabled = false;

            var rb = GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;

            UnityEngine.Object.Destroy(gameObject, destroyDelay);
            destroyed = true;
            
            return TaskStatus.Success;
        }
        

        return TaskStatus.Running;
    }
}