using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;


namespace RPG.MotionSystem
{
    //动画播放层
    public class PlayerAnim
    {
        
        private PlayableGraph _graph;
        private readonly Mixer _mixer;
        //这个字典是把状态名切换成索引值，才能让mixer切换动画
        private readonly Dictionary<string,int> _animStateIndex;
        private BlendTree2D _moveAnim;
        
        public PlayerAnim(PlayerMotion motion, AnimSetting setting)
        {
            
            
            _graph = PlayableGraph.Create();
            
            _mixer = new Mixer(_graph);
            _animStateIndex = new Dictionary<string, int>();
            
            var idleAnim = new IdleAnim(_graph,setting.GetParam("Idle"));
            AddState("Idle",idleAnim);
            
            
            _moveAnim = new BlendTree2D(_graph,setting.GetParam("Move"));
            AddState("Move",_moveAnim);
            
            
            
            AnimHelper.SetOutput(_graph, motion.Model.GetComponent<Animator>(),_mixer);
            AnimHelper.Start(_graph);
        }
        
        


        public void Stop()
        {
            _graph.Destroy();
        }
        
        public void TransitionTo(string name)
        {
           _mixer.TransitionTo(_animStateIndex[name]); 
        }
        

        private void AddState(string name, AnimBehaviour anim)
        {
            _mixer.AddInput(anim);
            _animStateIndex.Add(name,_mixer.inputCount - 1);
        }
        
    } 
}

