using System;
using UnityEngine;
using System.Collections.Generic;


namespace RPG.Animation
{
    [Serializable]
    public class AnimParam
    {
        public enum Type
        {
            Single,
            Group,
            InfoGroup,
            BlendClip,
        }

        public string name = "Anim";
        public Type type = Type.Single;
        public float enterTime;
        public AnimationClip clip;
        public AnimationClip[] clipGroup;
        public AnimInfo[] infoGroup;
        public BlendClip2D[] blendClip;
    }

    [Serializable]
    public class AnimInfo
    {
        public AnimationClip clip;
        public float enterTime;
    }
    
    [CreateAssetMenu(fileName = "New Anim Setting", menuName = "Game/Animation/Anim Setting")]
    public class AnimSetting : ScriptableObject
    {
        public List<AnimParam> animParams;

        public AnimParam GetParam(string name)
        {
            return animParams.Find(p => p.name == name);
        }
    }
}

