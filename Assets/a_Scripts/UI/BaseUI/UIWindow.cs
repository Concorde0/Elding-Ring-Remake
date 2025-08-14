using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public abstract class UIWindow
    { 
        public abstract UIBaseView View { get; }
        public abstract UIBaseViewModel ViewModel { get; }
    }
    
    public class UIWindow<TView, TViewModel> : UIWindow 
        where TView : UIBaseView 
        where TViewModel : UIBaseViewModel
    {
        public override UIBaseView View => _view;
        public override UIBaseViewModel ViewModel => _viewModel;

        private readonly TView _view;
        private readonly TViewModel _viewModel;

        public UIWindow(TView view, TViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }
    }
}

