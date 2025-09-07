using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BD_FindPlayerByTag : Action {
    public SharedString playerTag = "Player";
    public SharedTransform target;
    
    public override void OnAwake() {
        if (string.IsNullOrEmpty(playerTag.Value)) {
            playerTag.Value = "Player";
        }
    }

    public override TaskStatus OnUpdate() {
        var go = GameObject.FindGameObjectWithTag(playerTag.Value);
        if (go == null) return TaskStatus.Failure;
        target.Value = go.transform;
        return TaskStatus.Success;
    }
}
