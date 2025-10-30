using UnityEngine;

namespace QFramework
{

    public interface IItem
    {
        public string GetName { get; }
        public string GetKey { get; }
        public Sprite GetIcon { get; }
    }
}