using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public abstract class UIBaseViewModel
    {
        // 可在Initialize中订阅 Model或网络事件
        public virtual void Initialize() { }

        // 如果需要每帧更新，在UIManager中被调用
        public virtual void Update(float deltaTime) { }

        // 清理订阅、取消协程等
        public virtual void Dispose() { }
    }
}

