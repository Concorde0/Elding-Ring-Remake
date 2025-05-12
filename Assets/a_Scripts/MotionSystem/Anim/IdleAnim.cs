using System.Collections;
using System.Collections.Generic;
using RPG.Animation;
using UnityEngine;
using UnityEngine.Playables;



namespace RPG.MotionSystem
{
    public class IdleAnim : AnimBehaviour
    {
        private Mixer _mixer;
        public IdleAnim(PlayableGraph graph, float enterTime, AnimationClip[] clips) : base(graph, enterTime)
        {
            _mixer = new Mixer(graph);
            _adapterPlayable.AddInput(_mixer.GetAnimAdapterPlayable(),0,1f);

            var idleAnim = new AnimUnit(graph, clips[0], 0.5f);
            _mixer.AddInput(idleAnim);
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

