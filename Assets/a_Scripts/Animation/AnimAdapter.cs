using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.Animation
{
    public class AnimAdapter : PlayableBehaviour
    {
        private AnimBehaviour _animBehaviour;

        public void Init(AnimBehaviour animBehaviour)
        {
            this._animBehaviour = animBehaviour;
        }

        public void Enable()
        {
            _animBehaviour?.Enable();
        }

        public void Disable()
        {
            _animBehaviour?.Disable();
        }
        
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            // base.PrepareFrame(playable, info);
            _animBehaviour?.Execute(playable, info);
        }
    }

}
