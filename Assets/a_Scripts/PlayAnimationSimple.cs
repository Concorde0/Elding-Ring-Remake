using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class PlayAnimationSimple : MonoBehaviour
{
    public AnimationClip clip;
    private PlayableGraph _graph;
    private void Start()
    {
        _graph = PlayableGraph.Create();
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        
        var clipPlayable = AnimationClipPlayable.Create(_graph, clip);
        var output = AnimationPlayableOutput.Create(_graph, "Anim", GetComponent<Animator>());
        output.SetSourcePlayable(clipPlayable);
        
        
        _graph.Play();
    }

    private void OnDisable()
    {
        _graph.Destroy();
    }
}
