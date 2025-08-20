using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class UIManager
    {
        private readonly Dictionary<string, UIWindow> _windows = new Dictionary<string, UIWindow>();
        
        public void Show<TView, TViewModel>(string windowId)
            where TView : UIBaseView
            where TViewModel : UIBaseViewModel, new()
        {
            if (_windows.ContainsKey(windowId))
            {
                _windows[windowId].View.Show();
                return;
            }

            var prefabPath = UIWindowRegistry.GetPath(windowId);
            var prefab = Resources.Load<GameObject>(prefabPath);
            var instance = GameObject.Instantiate(prefab);
            
            var view = instance.GetComponent<TView>();
            var viewModel = new TViewModel();

            view.Initialize(viewModel);
            viewModel.Initialize();

            var window = new UIWindow<TView, TViewModel>(view, viewModel);
            _windows[windowId] = window;

            view.Show();
        }

        public void Hide(string windowId)
        {
            if (_windows.TryGetValue(windowId, out var window))
                window.View.Hide();
        }

        public void Close(string windowId)
        {
            if (_windows.TryGetValue(windowId, out var window))
            {
                window.View.Hide();
                window.ViewModel.Dispose();
                GameObject.Destroy(window.View.gameObject);
                _windows.Remove(windowId);
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var window in _windows.Values)
                window.ViewModel.Update(deltaTime);
        }
        
        //TODO:这里的IsOpen可以放在PlayerParam中
        public bool IsOpen(string windowId) => _windows.ContainsKey(windowId);
        
        public void Toggle<TView, TViewModel>(string windowId)
            where TView : UIBaseView
            where TViewModel : UIBaseViewModel, new()
        {
            if (IsOpen(windowId))
                Close(windowId);
            else
                Show<TView, TViewModel>(windowId);
        }
    }
}

