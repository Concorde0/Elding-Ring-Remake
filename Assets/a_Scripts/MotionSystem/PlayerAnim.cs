using System.Collections;
using System.Collections.Generic;
using RPG.Animation;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    //动画播放层
    public class PlayerAnim
    {
        private PlayableGraph _graph;
        private Mixer _mixer;
        //这个字典是把状态名切换成索引值，才能让mixer切换动画
        private Dictionary<string,int> _animMap;
        public PlayerAnim(PlayerMotion motion, AnimationClip[] clips)
        {
            _graph = PlayableGraph.Create();
            _mixer = new Mixer(_graph);
            _animMap = new Dictionary<string, int>();
            
            var idle = new IdleAnim(_graph, 0.5f,clips);
            AddState("Idle",idle);
            
            AnimHelper.SetOutput(_graph, motion.Model.GetComponent<Animator>(),_mixer);
            AnimHelper.Start(_graph);
        }
        
        


        public void Stop()
        {
            _graph.Destroy();
        }
        
        public void TransitionTo(string name)
        {
           _mixer.TransitionTo(_animMap[name]); 
        }

        private void AddState(string name, AnimBehaviour anim)
        {
            _mixer.AddInput(anim);
            _animMap.Add(name,_mixer.inputCount - 1);
        }
        
    } 
}

