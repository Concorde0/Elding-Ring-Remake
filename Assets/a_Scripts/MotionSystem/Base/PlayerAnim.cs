using System.Collections;
using System.Collections.Generic;
using RPG.AnimationSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
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
        private BlendTree2D _lockedMoveAnim;
        private BlendTree2D _moveAnim;
        private BlendTree2D _runAnim;
        private BlendTree2D _boilAnim;
        
        public PlayerAnim(PlayerMotion motion, AnimSetting setting)
        {
            
            //TODO:分离这里的所有注册方法，简化写法
            _graph = PlayableGraph.Create();
            
            _mixer = new Mixer(_graph);
            _animStateIndex = new Dictionary<string, int>();
            
            _lockedMoveAnim = new BlendTree2D(_graph,setting.GetParam(StringConstants.AnimName.LockedMove));
            AddState(StringConstants.AnimName.LockedMove,_lockedMoveAnim);
            
            _moveAnim = new BlendTree2D(_graph,setting.GetParam(StringConstants.AnimName.Move));
            AddState(StringConstants.AnimName.Move,_moveAnim);
            
            _runAnim = new BlendTree2D(_graph,setting.GetParam(StringConstants.AnimName.Run));
            AddState(StringConstants.AnimName.Run,_runAnim);
            
            var idleAnim = new SingleAnim(_graph,setting.GetParam(StringConstants.AnimName.Idle));
            AddState(StringConstants.AnimName.Idle,idleAnim);

            var boilAnim = new SingleAnim(_graph, setting.GetParam(StringConstants.AnimName.BoilForward));
            AddState(StringConstants.AnimName.BoilForward,boilAnim);
            
            var jumpBackward = new SingleAnim(_graph, setting.GetParam(StringConstants.AnimName.JumpBackward));
            AddState(StringConstants.AnimName.JumpBackward,jumpBackward);

            var moveStopAnim = new SingleAnim(_graph,setting.GetParam(StringConstants.AnimName.MoveStop));
            AddState(StringConstants.AnimName.MoveStop,moveStopAnim);
            
            var runStopAnim = new SingleAnim(_graph,setting.GetParam(StringConstants.AnimName.RunStop));
            AddState(StringConstants.AnimName.RunStop,runStopAnim);

            var attackAnim = new SingleAnim(_graph, setting.GetParam(StringConstants.AnimName.LightAttack1));
            AddState(StringConstants.AnimName.LightAttack1,attackAnim);
            
            
            
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

        public void SetMoveAnim(float x, float y)
        {
            _lockedMoveAnim.SetPointer(x,y);
            // _moveAnim.SetPointer(x,y);
            // _runAnim.SetPointer(x,y);
        }
        

        private void AddState(string name, AnimBehaviour anim)
        {
            _mixer.AddInput(anim);
            _animStateIndex.Add(name,_mixer.inputCount - 1);
        }
        
        public void EvaluateGraph(float deltaTime)
        {
            _graph.Evaluate(deltaTime); 
            // Debug.Log($"[Anim] EvaluateGraph called with deltaTime = {deltaTime} at time = {Time.time}");
        }

        
    } 
}

