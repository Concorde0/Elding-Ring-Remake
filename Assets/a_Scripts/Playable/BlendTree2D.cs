using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace RPG.AnimationSystem
{
    [Serializable]
    public struct BlendClip2D
    {
        public AnimationClip clip;
        public Vector2 pos;
    }

    public class BlendTree2D : AnimBehaviour
    {
        private AnimationMixerPlayable _mixer;
        private int _clipCount;

        // 平滑过渡字段
        private float[] _prevWeights;
        private float[] _targetWeights;
        private float[] _weightSpeed;
        private float   _blendTime = 0.1f;
        private float   _timeToNext;

        // ComputeShader 相关
        private ComputeShader _computeShader;
        private ComputeBuffer _computeBuffer;
        private int _kernel;
        private int _pointerXId;
        private int _pointerYId;
        private struct DataPair { public float x, y, output; }
        private DataPair[] _dataArray;

        public BlendTree2D(PlayableGraph graph, BlendClip2D[] clips, float enterTime = 0f)
            : base(graph, enterTime)
        {
            _clipCount = clips.Length;

            // 初始化数组
            _prevWeights   = new float[_clipCount];
            _targetWeights = new float[_clipCount];
            _weightSpeed   = new float[_clipCount];
            _dataArray     = new DataPair[_clipCount];

            // 初始化 ComputeShader
            _computeShader = AnimHelper.GetComputer("Blend2D");
            _computeBuffer = new ComputeBuffer(_clipCount, sizeof(float)*3);
            _kernel = _computeShader.FindKernel("Compute");
            _pointerXId = Shader.PropertyToID("pointerX");
            _pointerYId = Shader.PropertyToID("pointerY");
            _computeShader.SetFloat("eps", 1e-5f);
            _computeShader.SetBuffer(_kernel, "dataBuffer", _computeBuffer);

            // 创建 Mixer
            _mixer = AnimationMixerPlayable.Create(graph, 0);
            _adapterPlayable.AddInput(_mixer, 0, 1f);

            // 填充剪辑和采样点
            for (int i = 0; i < _clipCount; i++)
            {
                var c = clips[i];
                _dataArray[i].x = c.pos.x;
                _dataArray[i].y = c.pos.y;
                _dataArray[i].output = 0f;

                var clipPlayable = AnimationClipPlayable.Create(graph, c.clip);
                _mixer.AddInput(clipPlayable, 0, 0f);
            }

            // 初始权重
            ComputeWeights(Vector2.zero);
            Array.Copy(_targetWeights, _prevWeights, _clipCount);
            ApplyWeights(_prevWeights);
        }
        public BlendTree2D(PlayableGraph graph, AnimParam param) : this(graph, param.blendClip, param.enterTime) { }

        /// <summary>
        /// 设置新的二维指针(x,y)，开始平滑过渡
        /// </summary>
        public void SetPointer(float x, float y)
        {
            // 缓存当前权重
            for (int i = 0; i < _clipCount; i++)
                _prevWeights[i] = _mixer.GetInputWeight(i);

            // 计算目标权重
            ComputeWeights(new Vector2(x, y));

            // 计算速率
            _timeToNext = _blendTime;
            for (int i = 0; i < _clipCount; i++)
                _weightSpeed[i] = (_targetWeights[i] - _prevWeights[i]) / _blendTime;
        }

        public override void Execute(Playable playable, FrameData info)
        {
            base.Execute(playable, info);
            if (!enable) return;

            if (_timeToNext > 0f)
            {
                float dt = (float)info.deltaTime;
                _timeToNext -= dt;

                // 插值权重
                for (int i = 0; i < _clipCount; i++)
                {
                    float w = _mixer.GetInputWeight(i) + _weightSpeed[i] * dt;
                    _mixer.SetInputWeight(i, Mathf.Clamp01(w));
                }

                // 结束时强制设置目标权重
                if (_timeToNext <= 0f)
                    ApplyWeights(_targetWeights);
            }
        }

        /// <summary>
        /// 调用 ComputeShader 计算原始逆距离权重并归一化
        /// </summary>
        private void ComputeWeights(Vector2 pointer)
        {
            // 1) 让 ComputeShader 写出 raw outputs
            _computeShader.SetFloat(_pointerXId, pointer.x);
            _computeShader.SetFloat(_pointerYId, pointer.y);
            _computeBuffer.SetData(_dataArray);

            int groups = Mathf.CeilToInt(_clipCount / 16f);
            _computeShader.Dispatch(_kernel, groups, 1, 1);
            _computeBuffer.GetData(_dataArray);

            // 2) 找到 output 值最顶的三个索引
            //    我们先把 (index, output) 收集到列表里：
            var list = new List<(int idx, float val)>(_clipCount);
            for (int i = 0; i < _clipCount; i++)
                list.Add((i, _dataArray[i].output));

            // 排一下，降序
            list.Sort((a,b) => b.val.CompareTo(a.val));

            // 取前三（如果少于3，就取全部）
            int take = Math.Min(3, _clipCount);
            var topIndices = new HashSet<int>();
            for (int i = 0; i < take; i++)
                topIndices.Add(list[i].idx);

            // 3) 清零其它通道的 raw output，并计算 these 三个的总和
            float sum = 0f;
            for (int i = 0; i < _clipCount; i++)
            {
                if (topIndices.Contains(i))
                    sum += _dataArray[i].output;
                else
                    _dataArray[i].output = 0f;
            }

            // 4) 最后归一化，只对三条通道赋权重
            for (int i = 0; i < _clipCount; i++)
                _targetWeights[i] = (sum > 0f && _dataArray[i].output > 0f)
                    ? _dataArray[i].output / sum
                    : 0f;
        }

        private void ApplyWeights(float[] weights)
        {
            for (int i = 0; i < _clipCount; i++)
                _mixer.SetInputWeight(i, weights[i]);
        }

        public override void Stop()
        {
            base.Stop();
            _computeBuffer.Dispose();
        }
    }
}
