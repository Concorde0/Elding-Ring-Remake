using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public abstract class UIBaseView : MonoBehaviour
    {
        protected UIBaseViewModel ViewModel;
        
        //这里让Init在UIManager中由UIManager注入
        //但是为什么不用构造函数呢？因为Unity的MonoBehaviour不支持构造函数注入
        public void Initialize(UIBaseViewModel viewModel)
        {
            ViewModel = viewModel;
            BindEvents();
            OnInitialized();
        }
        
        
        protected abstract void BindEvents();
        protected abstract void UnbindEvents();
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnHide();
        }
        
        protected virtual void OnInitialized() { }
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }
        

        private void OnDestroy()
        {
            UnbindEvents();
        }
    }
}

