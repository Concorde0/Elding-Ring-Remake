using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    public class IdleAnim : AnimBehaviour
    {
        private readonly Mixer _mixer;
        public IdleAnim(PlayableGraph graph, float enterTime, AnimationClip clips) : base(graph, enterTime)
        {
            _mixer = new Mixer(graph);
            _adapterPlayable.AddInput(_mixer.GetAnimAdapterPlayable(),0,1f);

            var idleAnim = new AnimUnit(graph, clips, 0.5f);
            _mixer.AddInput(idleAnim);
        }

        public IdleAnim(PlayableGraph graph, AnimParam param) : this(graph, param.enterTime, param.clip)
        {
            
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
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

