using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


namespace RPG.Animation
{
    public class AnimUnit : AnimBehaviour
    {
        //用于保存clipPlayable的节点实例，便于管理
        private AnimationClipPlayable _anim;
        
        public AnimUnit(PlayableGraph graph, AnimationClip clip) : base(graph)
        {
            _anim = AnimationClipPlayable.Create(graph, clip);
            _adapterPlayable.AddInput(_anim,0,1f);
            
            Disable();
        }

        public override void Enable()
        {
            base.Enable();
            _adapterPlayable.SetTime(0f);
            _anim.SetTime(0f);
            _anim.Play();
            _adapterPlayable.Play();
        }

        public override void Disable()
        {
            base.Disable();
            _anim.Pause();
            _adapterPlayable.Pause();
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            
        }
    }
}

