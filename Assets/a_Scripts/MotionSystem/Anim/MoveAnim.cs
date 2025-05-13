using System.Collections;
using System.Collections.Generic;
using RPG.Animation;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.MotionSystem
{
    public class MoveAnim : AnimBehaviour
    {
        private readonly Mixer _mixer;

        public MoveAnim(PlayableGraph graph, float enterTime, AnimationClip[] clips) : base(graph, enterTime)
        {
            _mixer = new Mixer(graph);
            _adapterPlayable.AddInput(_mixer.GetAnimAdapterPlayable(), 0, 1f);

            for (int i = 0; i < clips.Length; i++)
            {
                var moveAnim = new AnimUnit(graph, clips[i], 0.5f);
                _mixer.AddInput(moveAnim);
            }

        }

        public MoveAnim(PlayableGraph graph, AnimParam param) : this(graph, param.enterTime,param.clipGroup)
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
}

