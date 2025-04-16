using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class MixAnimationSample : MonoBehaviour
{
    public AnimationClip clip1,clip2;
    [Range(0f,1f)]
    public float weight;
    
    private PlayableGraph graph;
    private AnimationMixerPlayable mixer;

    private void Start()
    {
        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        //此处注释是因为下面用了简便方法去连接节点
        // mixer = AnimationMixerPlayable.Create(graph, 2);
        mixer = AnimationMixerPlayable.Create(graph);
        //这里的mixer赋值有两种写法，在不知道具体数字的时候用第二种更方便
        // mixer = AnimationMixerPlayable.Create(graph);
        // mixer.SetInputCount(2);
        
        var ClipPlayable1 = AnimationClipPlayable.Create(graph, clip1);
        var ClipPlayable2 = AnimationClipPlayable.Create(graph, clip2);

        // graph.Connect(ClipPlayable1, 0, mixer, 0);
        // graph.Connect(ClipPlayable2, 0, mixer, 1);
        //第二种简便的方法去连接节点
        mixer.AddInput(ClipPlayable1, 0, 1f);
        mixer.AddInput(ClipPlayable2, 0, 0f);
        
        mixer.SetInputWeight(0, 1f);
    
        
        var output  = AnimationPlayableOutput.Create(graph, "Anim", GetComponent<Animator>());
        output.SetSourcePlayable(mixer);
        graph.Play();

    }

    private void Update()
    {
        mixer.SetInputWeight(0, 1 - weight);
        mixer.SetInputWeight(1, weight);
    }

    private void OnDisable()
    {
        graph.Destroy();
    }
}
