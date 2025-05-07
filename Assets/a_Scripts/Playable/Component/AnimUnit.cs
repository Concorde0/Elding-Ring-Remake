using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


namespace RPG.Animation
{
    public class AnimUnit : AnimBehaviour
    {
        //为了便于管理，用变量保存这个ClipPlayable
        private readonly AnimationClipPlayable _anim;

        public AnimUnit(PlayableGraph graph, AnimationClip clip, float enterTime = 0f) : base(graph,enterTime)
        {
            //将动画片段连接到Adapter上
            _anim = AnimationClipPlayable.Create(graph, clip);
            _adapterPlayable.AddInput(_anim, 0, 1f);
        }

        public override void Enable()
        {
            base.Enable();
            _adapterPlayable.Play();
            _anim.Play();
            _anim.SetTime(0);
            _adapterPlayable.SetTime(0);
        }

        public override void Disable()
        {
            base.Disable();
            _adapterPlayable.Pause();
            _anim.Pause();
            
        }
        
        
    }
    
    
}

