using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class UIManager
    {
        private readonly Dictionary<string, UIWindow> _windows = new Dictionary<string, UIWindow>();
        private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        
        public TViewModel Show<TView, TViewModel>(string windowId)
            where TView : UIBaseView
            where TViewModel : UIBaseViewModel, new()
        {
            
            if (_windows.TryGetValue(windowId, out var existing))
            {
                if (existing.View is TView && existing.ViewModel is TViewModel)
                {
                    existing.View.gameObject.SetActive(true);
                    return (TViewModel)existing.ViewModel;
                }
                Close(windowId);
            }

            var prefab = LoadPrefab(windowId);
            var instance  = UnityEngine.Object.Instantiate(prefab);
            var view      = instance.GetComponent<TView>();
            var viewModel = new TViewModel();
            

            view.Initialize(viewModel);
            viewModel.Initialize();

            _windows[windowId] = new UIWindow(view, viewModel);
            return viewModel;
        }
        
        public void Hide(string windowId)
        {
            if (_windows.TryGetValue(windowId, out var window))
                window.View.gameObject.SetActive(false);
        }
        public void Close(string windowId)
        {
            if (_windows.TryGetValue(windowId, out var window))
            {
                window.View.Hide();
                (window.ViewModel as IDisposable)?.Dispose();
                GameObject.Destroy(window.View.gameObject);
                _windows.Remove(windowId);
            }
        }
        
        public TViewModel Toggle<TView, TViewModel>(string windowId)
            where TView : UIBaseView
            where TViewModel : UIBaseViewModel, new()
        {
            if (_windows.TryGetValue(windowId, out var window) && window.View.gameObject.activeSelf)
            {
                Close(windowId);
                return null;
            }
            return Show<TView, TViewModel>(windowId);
        }

        public void Update(float deltaTime)
        {
            foreach (var window in _windows.Values)
                window.ViewModel.Update(deltaTime);
        }

        private GameObject LoadPrefab(string windowId)
        {
            var path = UIWindowRegistry.GetPath(windowId);
            if (_prefabCache.TryGetValue(path, out var cached))
                return cached;

            var prefab = Resources.Load<GameObject>(path);
            if (prefab != null)
                _prefabCache[path] = prefab;
            return prefab;
        }

        //一对 View + ViewModel
        private class UIWindow
        {
            public UIBaseView View       { get; }
            public UIBaseViewModel ViewModel { get; }
            public UIWindow(UIBaseView view, UIBaseViewModel vm)
            {
                View = view;
                ViewModel = vm;
            }
        }
    }
}
