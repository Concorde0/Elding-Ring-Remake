using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class PlayAnimationSample : MonoBehaviour
{
    
    public AnimationClip clip;
    private PlayableGraph graph;  //在用graph时，要注册和注销，并且要在想要的位置进行graph.Play();
    private AnimationClipPlayable clipPlayable;

    private void Start()
    {
        graph = PlayableGraph.Create();
        //设置时间更新模式，跟随游戏时间进行更新
        graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        
        //这是一个可播放的动画剪辑节点
        var clipPlayable = AnimationClipPlayable.Create(graph, clip);
        
        //这是一个输出节点
        var output = AnimationPlayableOutput.Create(graph, "Anim", GetComponent<Animator>());
        //将刚创建的 AnimationClipPlayable 作为输出节点的源，这样 Animator 就会播放由 clipPlayable 管理的动画剪辑。
        output.SetSourcePlayable(clipPlayable);
        graph.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (clipPlayable.GetPlayState() == PlayState.Playing)
            {
                clipPlayable.Pause();
            }
            else
            {
                clipPlayable.Play();
                clipPlayable.SetTime(0f);
            }
        }
    }


    private void OnDisable()
    {
        graph.Destroy();
    }
}
