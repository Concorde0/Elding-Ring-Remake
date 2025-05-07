using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.Animation
{
    
    //需要通过 AnimAdapter 来控制 AnimBehaviour 的生命周期和状态，所以要在Enable和Disable中重新调用Behaviour中的方法
    //保证封装性，封装AnimBehaviour
    //总结就是 在 AnimAdapter 中加 Enable() / Disable()，是为了 封装 AnimBehaviour 的控制接口，让外部代码只和 Adapter 接触，保持职责清晰。
    public class AnimAdapter : PlayableBehaviour
    {
        private AnimBehaviour _animBehaviour;

        public void Init(AnimBehaviour animBehaviour)
        {
            _animBehaviour = animBehaviour;
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
            _animBehaviour.Execute(playable,info);
        }

        public float GetAnimEnterTime()
        {
            return _animBehaviour.GetEnterTime();
        }
    }

}
