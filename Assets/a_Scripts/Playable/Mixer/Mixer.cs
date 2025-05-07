using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


namespace RPG.Animation
{
    public class Mixer : AnimBehaviour
    {
        public int inputCount { get; private set; } //当前已加入混合器的动画路数
        public int currentIndex => _currentIndex;
        public bool isTransition => _isTransition;
        
        private AnimationMixerPlayable _mixer;
        
        private float _timeToNext;
        private bool _isTransition;
        private int _targetIndex; //正在过渡进入的目标动画索引；如果不在过渡中，为 -1
        private int _currentIndex; //当前动画索引
        private float _currentSpeed;
        
        private readonly List<int> _declinedIndex; //正在淡出的旧动画索引列表
        private float _declinedSpeed; //其他被加进淡出列表的动画权重衰减速度
        private float _declinedWeight; //用来累加当前所有正在淡出的动画的权重
        
        
        public Mixer(PlayableGraph graph) : base(graph)
        {
            //这里为什么要加额外的两个参数呢？
            //第二个参数 0，表示 初始时 Mixer 没有任何输入端口。
            //第三个参数 true，表示 自动归一化权重（保证加起来等于 1），这样你后面就不用手动去 normalize。
            //我操，第三个参数被unity弃用了
            _mixer = AnimationMixerPlayable.Create(graph,0);
            //adapter应该是要连接Mixer,再连到addInput的吧？
            //把这个 Mixer 挂到外层的 AdapterPlayable 上
            _adapterPlayable.AddInput(_mixer,0,1f);
            
            
            _declinedIndex = new List<int>();
            _targetIndex = -1;
        }

        //AddInput 的作用是  往 Mixer 里插入你要混合的那些动画
        public override void AddInput(Playable playable)
        {
            base.AddInput(playable);
            _mixer.AddInput(playable,0,0f);
            //这里inputCount++的意思是，每一个动画都必须有一个单独的动画端口，每次addInput就把当前动画端口+1
            inputCount++;
            //如果当前的端口正好等于1，那就把他的权重设为1，让他播放(应该是可选吧？)
            if (inputCount == 1)
            {
                _mixer.SetInputWeight(0,1f);
                _currentIndex = 0;
            }

        }
        

        public override void Enable()
        {
            base.Enable();

            if (inputCount > 0)
            {
                AnimHelper.Enable(_mixer,0);
            }
            _mixer.SetTime(0f);
            _mixer.Play();
            _adapterPlayable.SetTime(0f);
            _adapterPlayable.Play();
            
            _mixer.SetInputWeight(0,1f);

            _currentIndex = 0;
            _targetIndex = -1;
        }

        public override void Disable()
        {
            base.Disable();
            for (var i = 0; i < inputCount; i++)
            {
                _mixer.SetInputWeight(i,0f);
                AnimHelper.Disable(_mixer,i);
            }
            
            _mixer.Pause();
            _adapterPlayable.Pause();
        }
        
        
        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            if(!enable) return;
            if(!_isTransition || _targetIndex < 0) return;

            if (_timeToNext > 0f)
            {
                _timeToNext -= info.deltaTime;

                _declinedWeight = 0f; 
                for (int i = 0; i < _declinedIndex.Count; i++)
                {
                    var w = ModifyWeight(_declinedIndex[i], -info.deltaTime * _declinedSpeed);
                    if (w <= 0f)
                    {
                        AnimHelper.Disable(_mixer,_declinedIndex[i]);
                        _declinedIndex.Remove(_declinedIndex[i]);
                    }
                    else
                    {
                        _declinedWeight += w;
                    }
                }

                _declinedWeight += ModifyWeight(_currentIndex, -info.deltaTime * _currentSpeed);
                SetWeight(_targetIndex,1f - _declinedWeight);
                return;
            }

            _isTransition = false;
            
            AnimHelper.Disable(_mixer,_currentIndex);
            _currentIndex = _targetIndex;
            _targetIndex = -1;
        }
        
        
        //index代表想要过度到的下一个动画
        //targetIndex代表当前动画的想要去的过渡状态，也代表了动画何时转换完毕，如果过渡完毕，targetIndex=index
        //currentIndex代表当前动画的过渡状态
        
        // 我操你妈吧，这还是个倒叙呢
        // 比如我第一次播放动画，current是走路，target是attack1。
        // 如果当前的过度状态是走路>attack1，我再次输入attack2，他就会取消attack1转而变成走路过度为attack2
        // 如果当前的过度状态时走路<attack1，我再次输入attack2，他就会从attack1过渡为attack2
        public void TransitionTo(int index)
        {  
            if (_isTransition && _targetIndex >= 0)
            {
                if(index == _targetIndex) return;
                if (index == _currentIndex)
                {
                    _currentIndex = _targetIndex;
                }
                else if(GetWeight(_currentIndex) > GetWeight(_targetIndex))
                {
                    _declinedIndex.Add(_targetIndex);
                }
                else
                {
                    _declinedIndex.Add(_currentIndex);
                    _currentIndex = _targetIndex;
                }
            }
            else
            {
                if(index == _currentIndex) return;
            }
            
            _targetIndex = index;
            _declinedIndex.Remove(_targetIndex);
            AnimHelper.Enable(_mixer,_targetIndex);
            
            _timeToNext = GetTargetEnterTime(_targetIndex) * (1f - GetWeight(_targetIndex));
            _declinedSpeed = 2f/_timeToNext;
            _currentSpeed = GetWeight(_currentIndex) / _timeToNext;
            
            _isTransition = true;

        }

        public float GetWeight(int index)
        {
            if (index >= 0 && index < _mixer.GetInputCount())
            {
                return _mixer.GetInputWeight(index);
            }
            else
            {
                return 0;
            }
        }

        private void SetWeight(int index, float weight)
        {
            if (index >= 0 && index < _mixer.GetInputCount())
            {
                _mixer.SetInputWeight(index, weight);
            }
                
        }

        private float GetTargetEnterTime(int index)
        {
            return ((ScriptPlayable<AnimAdapter>)_mixer.GetInput(index)).GetBehaviour().GetAnimEnterTime();
        }
        
        
        //修改指定动画通道的权重
        private float ModifyWeight(int index, float delta)
        {
            if (index < 0 || index >= inputCount)
            {
                return 0;
            }
            float weight = Mathf.Clamp01(_mixer.GetInputWeight(index) + delta);
            _mixer.SetInputWeight(index, weight);
            return weight;
        }
    }
}

