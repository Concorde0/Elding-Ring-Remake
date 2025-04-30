using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;


namespace RPG.Animation
{
    public class AnimTest : MonoBehaviour
    {
        private PlayableGraph _graph;
        public AnimationClip clip;
        private AnimUnit anim;

        private void Start()
        {
            _graph = PlayableGraph.Create();
            _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            anim = new AnimUnit(_graph, clip);
            var output = AnimationPlayableOutput.Create(_graph, "Anim", GetComponent<Animator>());
            output.SetSourcePlayable(anim.GetAnimAdapterPlayable());
            
            _graph.Play();
            anim.Enable();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (anim.enable)
                {
                    anim.Disable();
                }
                
                else
                {
                    anim.Enable();
                }
               
            }
            
        }

        private void OnDisable()
        {
            _graph.Destroy();
        }
    }
}

