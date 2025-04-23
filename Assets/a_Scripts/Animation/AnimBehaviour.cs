using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.Animation
{
    public abstract class AnimBehaviour
    {
        public bool enable { get;private set; }
        protected Playable _adapterPlayable;

        public AnimBehaviour()
        {
            
        }
        
        public AnimBehaviour(PlayableGraph graph)
        {
            _adapterPlayable = ScriptPlayable<AnimAdapter>.Create(graph);
            ((ScriptPlayable<AnimAdapter>)_adapterPlayable).GetBehaviour().Init(this);
        }

        public virtual void Enable()
        {
            enable = true;
        }

        public virtual void Disable()
        {
            enable = false;
        }

        public virtual void Execute(Playable playable, FrameData info)
        {
            if (!enable)
            {
                return;
            }
        }

        public Playable GetAnimAdapterPlayable()
        {
            return _adapterPlayable;
        }
    }
}

