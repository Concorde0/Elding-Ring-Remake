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
    private int currentClip;

    public void Init(PlayableGraph graph, Playable owner, AnimationClip[] clips)
    {
        //字面意思，设置一个输入端口
        owner.SetInputCount(1);
        //creat一个mixerPlayable
        mixer = AnimationMixerPlayable.Create(graph);
        //把Clip里面的所有动画Add到mixer里面
        for (int i = 0; i < clips.Length; ++i)
        {
            mixer.AddInput(AnimationClipPlayable.Create(graph, clips[i]),0);
        }
        //为mixer设置权重，index为0的动画设置为1的权重
        mixer.SetInputWeight(0,1f);
        //graph.Connect(source, sourceOutputPort, destination, destinationInputPort);
        //源头为Mixer，index为0，连接到owner，目标的index也为0
        graph.Connect(mixer,0,owner,0);
        
        //默认动画时间为第一个动画的时长
        timeToNext = clips[0].length;
    }
    
    // PrepareFrame函数会在每帧动画播放前调用
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);

        timeToNext -= info.deltaTime;
        //GetInputCount能够获取到目前的输入端口数量
        if (timeToNext < 0f && currentClip < mixer.GetInputCount() - 1)
        {
            //改变权重，让播放完的动画权重为0，下一个要播放的动画权重为1
            mixer.SetInputWeight(currentClip,0f);
            mixer.SetInputWeight(currentClip + 1,1f);
            
            currentClip++;
            mixer.GetInput(currentClip).SetTime(0f);
            
            timeToNext = ((AnimationClipPlayable) mixer.GetInput(currentClip)).GetAnimationClip().length;
        }
    }
}
public class PlayableQueueSample : MonoBehaviour
{
    
    public AnimationClip[] clips;
        
    private PlayableGraph graph;


    private void Start()
    {
        graph = PlayableGraph.Create();
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        //这里的ScriptPlayable是将自定义的类<T>与Playable相关联，在指定的 PlayableGraph 中创建了一个新的 ScriptPlayable 实例，
        //封装了 PlayableQueue 行为。随后，可以通过 queuePlayable.GetBehaviour() 获取该行为实例，并进行初始化或其他操作。
        //关键词：与Playable关联，在Graph中嵌入自定义的逻辑
        var queuePlayable = ScriptPlayable<PlayableQueue>.Create(graph);
        var queue = queuePlayable.GetBehaviour();
        queue.Init(graph,queuePlayable,clips);
        
        var output = AnimationPlayableOutput.Create(graph, "Anim", GetComponent<Animator>());
        //指定queuePlayable为output的源
        output.SetSourcePlayable(queuePlayable);
        graph.Play();
    }

    private void OnDisable()
    {
        graph.Destroy();
    }
}
