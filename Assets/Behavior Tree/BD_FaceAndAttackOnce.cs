using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_FaceAndAttackOnce : Action {
    public SharedTransform target;
    public float turnSpeed = 10f;   // 角速度(简易)
    public float attackCooldown = 1.0f;
    public string message = "OnEnemyAttack"; // 或 Animator Trigger

    private float timer;

    public override void OnStart() {
        timer = 0f;
        // 触发一次攻击事件（你可改成 Animator.SetTrigger 或自定义系统）
        gameObject.SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }

    public override TaskStatus OnUpdate() {
        if (target.Value != null) {
            Vector3 dir = target.Value.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f) {
                var want = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation, want, turnSpeed * 360f * Time.deltaTime);
            }
        }
        timer += Time.deltaTime;
        return (timer >= attackCooldown) ? TaskStatus.Success : TaskStatus.Running;
    }
}