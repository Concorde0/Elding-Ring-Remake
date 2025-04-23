using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


public class PlayableQueue : PlayableBehaviour
{
    private AnimationMixerPlayable mixer;
    private float timeToNext;
    private int currentClipIndex;
    public void Init(PlayableGraph graph, Playable owner, AnimationClip[] clips)
    {
        //我在这里加一句这个就正常了，为什么
        owner.SetInputCount(1);
        
        mixer = AnimationMixerPlayable.Create(graph);
        for (int i = 0; i < clips.Length; ++i)
        {
            var animClip = AnimationClipPlayable.Create(graph, clips[i]);
            mixer.AddInput(animClip,0,0f);
        }
        mixer.SetInputWeight(0,1f);
        //为什么要connect
        graph.Connect(mixer,0,owner,0);
        timeToNext = clips[0].length;
        
        
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        //这里为什么用Info
        base.PrepareFrame(playable, info);
        timeToNext -= info.deltaTime;
        if (timeToNext < 0 && currentClipIndex < mixer.GetInputCount() - 1)
        {
            mixer.SetInputWeight(currentClipIndex,0f);
            mixer.SetInputWeight(currentClipIndex+1,1f);
            currentClipIndex++;
            mixer.GetInput(currentClipIndex).SetTime(0f);
            timeToNext = ((AnimationClipPlayable)mixer.GetInput(currentClipIndex)).GetAnimationClip().length;
            
        }
    }
}

public class PlayableQueueSample : MonoBehaviour
{
    private PlayableGraph _graph;
    public AnimationClip[] clips;
    private void Start()
    {
        _graph = PlayableGraph.Create();
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        var playableQueue = ScriptPlayable<PlayableQueue>.Create(_graph);
        var queue = playableQueue.GetBehaviour();
        queue.Init(_graph, playableQueue, clips);
        
        var output = AnimationPlayableOutput.Create(_graph, "Anim", GetComponent<Animator>());
        output.SetSourcePlayable(playableQueue);
        
        
        _graph.Play();
    }

    private void OnDisable()
    {
        _graph.Destroy();
    }
}
