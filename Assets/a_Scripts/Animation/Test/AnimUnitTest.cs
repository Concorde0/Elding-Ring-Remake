using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


namespace RPG.Animation
{
    public class AnimUnitTest : MonoBehaviour
    {
        private PlayableGraph _graph;
        public AnimationClip clip;
        private AnimUnit _anim;

        private void Start()
        {
            _graph = PlayableGraph.Create();
            
            _anim = new AnimUnit(_graph, clip);

            var output = AnimationPlayableOutput.Create(_graph, "Anim", GetComponent<Animator>());
            output.SetSourcePlayable(_anim.GetAnimAdapterPlayable());
            
            _graph.Play();
            // _anim.Enable();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_anim.enable)
                {
                    _anim.Disable();
                }
                else
                {
                    _anim.Enable();
                }
            }
        }

        private void OnDisable()
        {
            _graph.Destroy();
        }
    }
}

