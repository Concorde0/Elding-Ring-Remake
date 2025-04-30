using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.Animation
{
    //写这个 AnimBehaviour，是为了创建一个自己的动画状态逻辑系统!!
    public abstract class AnimBehaviour
    {
        public bool enable{get;private set;}
        public float remainTime { get; protected set; }
        
        
        protected float _enterTime;
        protected float _animLength;
        protected Playable _adapterPlayable;
        
        
        public AnimBehaviour(float enterTime = 0f) { _enterTime = enterTime; }
        public AnimBehaviour(PlayableGraph graph, float enterTime = 0f)
        {
            //保存Adapter的节点在_adapterPlayable中，目的是为了让外部能用统一的 Playable 类型做连接、混合等操作
            _adapterPlayable = ScriptPlayable<AnimAdapter>.Create(graph);
            //把 _adapterPlayable强制转换成scriptPlayable，实例化之后把AnimBehaviour穿到Adapter中，依赖注入
            ((ScriptPlayable<AnimAdapter>)_adapterPlayable).GetBehaviour().Init(this);
            
            _enterTime = enterTime;
        }
        

        public virtual void Enable()
        {
            if (enable) return;
            enable = true;
            remainTime = GetAnimLength();
        }

        public virtual void Disable()
        {
            if (!enable) return;
            enable = false; 
        }
        
        public virtual void Stop() { }

        public virtual void Execute(Playable playable, FrameData info)
        {
            if (!enable) return;
        }
        

        public virtual void AddInput(Playable playable) { }
        public void AddInput(AnimBehaviour behaviour) 
        {
            AddInput(behaviour.GetAnimAdapterPlayable());
        }
        
        
        //这里在Anim独立脚本中，可以通过AnimUnit在调用这个函数，从而获取到Adapter,以此来绑定output
        public Playable GetAnimAdapterPlayable()
        {
            return _adapterPlayable;
        }
        
        public virtual float GetEnterTime()
        {
            return _enterTime;
        }
        
        public virtual float GetAnimLength()
        {
            return _animLength;
        }
    }
}

