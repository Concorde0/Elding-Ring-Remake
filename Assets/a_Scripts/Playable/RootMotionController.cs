using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace RPG.AnimationSystem
{
    public class RootMotionController
    {
        private struct ClipMotionData
        {
            public Vector3 position;
            public Quaternion rotation;
            public float weight;
        }

        private readonly Transform _targetTransform;
        private readonly AnimationMixerPlayable _mixer;
        private ClipMotionData[] _clipData;
        private Vector3 _lastPosition;
        private Quaternion _lastRotation;

        public RootMotionController(Transform target, AnimationMixerPlayable mixer)
        {
            _targetTransform = target;
            _mixer = mixer;
            InitializeClipData();
        }

        private void InitializeClipData()
        {
            int clipCount = _mixer.GetInputCount();
            _clipData = new ClipMotionData[clipCount];
            
            for (int i = 0; i < clipCount; i++)
            {
                var clipPlayable = (AnimationClipPlayable)_mixer.GetInput(i);
                _clipData[i] = new ClipMotionData
                {
                    position = Vector3.zero,
                    rotation = Quaternion.identity,
                    weight = 0f
                };
            }
        }

        public void UpdateRootMotion()
        {
            Vector3 blendedPosition = Vector3.zero;
            Quaternion blendedRotation = Quaternion.identity;

            for (int i = 0; i < _clipData.Length; i++)
            {
                float weight = _mixer.GetInputWeight(i);
                var clipPlayable = (AnimationClipPlayable)_mixer.GetInput(i);
                
                SampleClipMotion(clipPlayable, ref _clipData[i], weight);
                
                blendedPosition += _clipData[i].position * weight;
                blendedRotation = Quaternion.Slerp(blendedRotation, _clipData[i].rotation, weight);
            }

            ApplyMotionDelta(blendedPosition, blendedRotation);
        }

        private void SampleClipMotion(AnimationClipPlayable clipPlayable, ref ClipMotionData data, float weight)
        {
            float normalizedTime = (float)(clipPlayable.GetTime() / clipPlayable.GetAnimationClip().length);
            
            GameObject tempSampler = new GameObject("TempSampler");
            try
            {
                clipPlayable.GetAnimationClip().SampleAnimation(tempSampler, normalizedTime);
                data.position = tempSampler.transform.localPosition;
                data.rotation = tempSampler.transform.localRotation;
                data.weight = weight;
            }
            finally
            {
                Object.Destroy(tempSampler);
            }
        }

        private void ApplyMotionDelta(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            // 实际应用逻辑（根据你的物理系统选择）
            // 示例使用CharacterController：
            // _characterController.Move(deltaPosition);
            // _targetTransform.rotation *= deltaRotation;
            
            // 或直接操作Transform：
            _targetTransform.position += deltaPosition;
            _targetTransform.rotation *= deltaRotation;
            
            // 重置动画根节点位置（关键步骤！）
            ResetClipRootPositions();
        }

        private void ResetClipRootPositions()
        {
            for (int i = 0; i < _clipData.Length; i++)
            {
                var clipPlayable = (AnimationClipPlayable)_mixer.GetInput(i);
                clipPlayable.GetAnimationClip().SampleAnimation(_targetTransform.gameObject, 0f);
            }
        }
    }
}