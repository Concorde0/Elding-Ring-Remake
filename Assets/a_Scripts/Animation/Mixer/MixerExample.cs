using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Serialization;

namespace RPG.Animation
{
    public class MixerExample : MonoBehaviour
    {
        public AnimationClip[] clips;

        private PlayableGraph _graph;
        private Mixer _mixer;

        private void Start()
        {
            _graph = PlayableGraph.Create();
            var animUnit1 = new AnimUnit(_graph, clips[0], 0.1f);
            var animUnit2 = new AnimUnit(_graph, clips[1], 0.1f);
            var animUnit3 = new AnimUnit(_graph, clips[2], 0.1f);
            _mixer = new Mixer(_graph);
            _mixer.AddInput(animUnit1);
            _mixer.AddInput(animUnit2);
            _mixer.AddInput(animUnit3);

            AnimHelper.SetOutput(_graph, GetComponent<Animator>(), _mixer);
            AnimHelper.Start(_graph);
            
        }

        private void Update()
        {
            if(UnityEngine.Input.GetKey(KeyCode.W))
            {
                _mixer.TransitionTo(0);
            }
            else if(UnityEngine.Input.GetKey(KeyCode.A))
            {
                _mixer.TransitionTo(1);
            }
            else if (UnityEngine.Input.GetKey(KeyCode.D))
            {
                _mixer.TransitionTo(2);
            }
        }

        private void OnDisable()
        {
            _graph.Destroy();
        }
    }
}  


