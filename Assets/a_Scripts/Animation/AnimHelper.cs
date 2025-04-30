using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.Animation
{
    public abstract class AnimHelper
    {
        public static void Enable(Playable playable)
        {
            var adapter = GetAdapter(playable);
            if (adapter != null)
            {
                adapter.Enable();
            }
        }
        
        public static void Start(PlayableGraph graph)
        {
            GetAdapter(graph.GetOutputByType<AnimationPlayableOutput>(0).GetSourcePlayable()).Enable();
            graph.Play();
        }
        
        //GetInput(int) return 当前索引的Playable
        //在AnimHelper重新写的Enable方法和Disable方法的本质就是，在clip被禁/启用的基础上，同时禁/启用AnimAdapter
        public static void Enable(AnimationMixerPlayable mixer, int index)
        {
            Enable(mixer.GetInput(index));
        }
        //这里方法重载的目的就是当有一个mixer需要用到Enable/Disable时，不需要再写多余的GetInput(int)来获取到当前索引的Playable，而是直接封装成一个重载的方法，便于利用
        public static void Disable(Playable playable)
        {
            var adapter = GetAdapter(playable);
            if (adapter != null)
            {
                adapter.Disable();
            }
            
        }
        public static void Disable(AnimationMixerPlayable mixer, int index)
        {
            Disable(mixer.GetInput(index));
        }

        //GetAdapter是能够获取到目标是否派生自Adapter，并且能够返回这个Adapter
        public static AnimAdapter GetAdapter(Playable playable)
        {
            if (typeof(AnimAdapter).IsAssignableFrom(playable.GetPlayableType()))
            {
                return ((ScriptPlayable<AnimAdapter>)playable).GetBehaviour();
            }
            return null;
        }
        

        public static void SetOutput(PlayableGraph graph, Animator animator, AnimBehaviour behaviour)
        {
            AnimationPlayableOutput.Create(graph,"Anim",animator).SetSourcePlayable(behaviour.GetAnimAdapterPlayable());
        }

        
        public static void Start(PlayableGraph graph, Animator animator, AnimBehaviour behaviour)
        {
            graph.Play();
            behaviour.Enable();
        }
    }
}

