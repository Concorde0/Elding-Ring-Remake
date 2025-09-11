using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


namespace RPG.AnimationSystem
{
    public class AnimUnit : AnimBehaviour
    {
        //为了便于管理，用变量保存这个 ClipPlayable
        private readonly AnimationClipPlayable _anim;

        public AnimUnit(PlayableGraph graph, AnimationClip clip, float enterTime = 0f) : base(graph,enterTime)
        {
            //将动画片段连接到 Adapter 上
            _anim = AnimationClipPlayable.Create(graph, clip);
            _adapterPlayable.AddInput(_anim, 0, 1f);
        }

        public override void Enable()
        {
            base.Enable();
            _anim.SetTime(0);
            _anim.Play();
            _adapterPlayable.SetTime(0);
            _adapterPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}

