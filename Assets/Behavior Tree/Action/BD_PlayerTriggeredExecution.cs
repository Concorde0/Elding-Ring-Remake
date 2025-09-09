using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_PlayerTriggeredExecution : Conditional
{
    public SharedTransform player;
    public float executionRange = 2f;
    public float executionWindow = 1.5f; // 处决窗口时间（秒）

    private CharacterStats stats;
    private float executionTimer = 0f;
    private bool isListening = false;

    public override void OnAwake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public override TaskStatus OnUpdate()
    {
        if (stats == null || player.Value == null || !stats.isExecution)
            return TaskStatus.Failure;

        float dist = Vector3.Distance(transform.position, player.Value.position);
        if (dist > executionRange)
            return TaskStatus.Failure;

        // 开始监听处决按键
        if (!isListening)
        {
            executionTimer = executionWindow;
            isListening = true;
        }

        // 倒计时监听窗口
        if (executionTimer > 0f)
        {
            executionTimer -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Successful Execution Triggered");
                stats.isExecution = false;
                isListening = false;
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        // 超时未触发
        isListening = false;
        return TaskStatus.Failure;
    }
}