using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class PlayAnimationMixerSimple : MonoBehaviour
{
    public AnimationClip clip1, clip2;
    private PlayableGraph _graph;
    private AnimationMixerPlayable mixer;
    [Range(0, 1f)] public float weight;

    private void Start()
    {
        _graph = PlayableGraph.Create();
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        // ✅ 初始化时指定 2 个输入通道
        mixer = AnimationMixerPlayable.Create(_graph, 2);

        var clipPlayable1 = AnimationClipPlayable.Create(_graph, clip1);
        var clipPlayable2 = AnimationClipPlayable.Create(_graph, clip2);

        // ✅ 用 Connect 来手动连接输入
        _graph.Connect(clipPlayable2, 0, mixer, 1);
        _graph.Connect(clipPlayable1, 0, mixer, 0);

        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);

        var output = AnimationPlayableOutput.Create(_graph, "Anim", GetComponent<Animator>());
        output.SetSourcePlayable(mixer);

        _graph.Play();
    }

    private void Update()
    {
        mixer.SetInputWeight(0, 1 - weight);
        mixer.SetInputWeight(1, weight);
    }

    private void OnDisable()
    {
        _graph.Destroy();
    }
}