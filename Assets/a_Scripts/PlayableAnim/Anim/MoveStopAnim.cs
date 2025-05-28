using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using UnityEngine;
using UnityEngine.Playables;

public class MoveStopAnim : AnimBehaviour
{
    private readonly Mixer _mixer;
    public MoveStopAnim(PlayableGraph graph, float enterTime, AnimationClip clips) : base(graph, enterTime)
    {
        _mixer = new Mixer(graph);
        _adapterPlayable.AddInput(_mixer.GetAnimAdapterPlayable(),0,1f);

        var moveStopAnim = new AnimUnit(graph, clips, 0.5f);
        _mixer.AddInput(moveStopAnim);
    }
    
    public MoveStopAnim(PlayableGraph graph, AnimParam param) : this(graph, param.enterTime, param.clip)
    {
            
    }
    
    public override void Enable()
    {
        base.Enable();
            
        _adapterPlayable.SetTime(0f);
        _adapterPlayable.Play();
        _mixer.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        _adapterPlayable.Pause();
        _mixer.Disable();
    }
}
