using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering;


namespace RPG.AnimationSystem
{
        [System.Serializable]
        public struct BlendClip2D
        {
            public AnimationClip clip;
            public Vector2 pos;
        }

        public class BlendTree2D : AnimBehaviour
        {
            private struct DataPair
            {
                public float x; 
                public float y;
                public float output;
            }

            private AnimationMixerPlayable _mixer;
            private Vector2 _pointer;
            private float _total;
            private int _clipCount;

            private ComputeShader _computeShader;
            private ComputeBuffer _computeBuffer;
            private DataPair[] _datas;
            private int _kernel;
            private int _pointerX;
            private int _pointerY;
            public BlendTree2D(PlayableGraph graph, BlendClip2D[] clips, float enterTime = 0f, float eps = 1e-5f) : base(graph, enterTime)
            {
                
                _datas = new DataPair[clips.Length];

                _mixer = AnimationMixerPlayable.Create(graph);
                _adapterPlayable.AddInput(_mixer, 0, 1f);
                
                for (int i = 0; i < clips.Length; i++)
                {
                    _mixer.AddInput(AnimationClipPlayable.Create(graph, clips[i].clip), 0);
                    _datas[i].x = clips[i].pos.x;
                    _datas[i].y = clips[i].pos.y;
                }

                _computeShader = AnimHelper.GetComputer("Blend2D");
                _computeBuffer = new ComputeBuffer(clips.Length, 12);
                _kernel = _computeShader.FindKernel("Compute");
                _computeShader.SetBuffer(_kernel, "dataBuffer", _computeBuffer);
                _computeShader.SetFloat("eps", eps);
                _pointerX = Shader.PropertyToID("pointerX");
                _pointerY = Shader.PropertyToID("pointerY");
                _clipCount = clips.Length;
                _pointer.Set(1, 1);
                SetPointer(0, 0);
                
            }
            
            public BlendTree2D(PlayableGraph graph, AnimParam param) : this(graph, param.blendClip, param.enterTime) { }
            

            public void SetPointer(Vector2 vector)
            {
                SetPointer(vector.x, vector.y);
            }

            public void SetPointer(float x, float y)
            {
                
                
                if (_pointer.x == x && _pointer.y == y)
                {
                    return;
                }

                _pointer.Set(x, y);

                int i;
                _computeShader.SetFloat(_pointerX, x);
                _computeShader.SetFloat(_pointerY, y);

                _computeBuffer.SetData(_datas);
                
                int threadGroupsX = Mathf.CeilToInt(_clipCount / 16.0f);
                _computeShader.Dispatch(_kernel, threadGroupsX, 1, 1);
                
                _computeBuffer.GetData(_datas);
                
                
                for (i = 0; i < _clipCount; i++)
                {
                    _total += _datas[i].output;
                }

                for (i = 0; i < _clipCount; i++)
                {
                    float normalizedWeight = ( _total > 0) ? _datas[i].output /  _total : 0;
                    _mixer.SetInputWeight(i, normalizedWeight);
                }

                _total = 0f;
            }

            public override void Enable()
            {
                base.Enable();

                SetPointer(0, 0);
                for (int i = 0; i < _clipCount; i++)
                {
                    _mixer.GetInput(i).Play();
                    _mixer.GetInput(i).SetTime(0f);
                }

                _mixer.SetTime(0f);
                _mixer.Play();
                _adapterPlayable.SetTime(0f);
                _adapterPlayable.Play();
                
            }

            public override void Disable()
            {
                base.Disable();
                for (int i = 0; i < _clipCount; i++)
                {
                    _mixer.GetInput(i).Pause();
                }

                _mixer.Pause();
                _adapterPlayable.Pause();
            }

            public override void Stop()
            {
                base.Stop();
                _computeBuffer.Dispose();
            }
            
            
        }
}



