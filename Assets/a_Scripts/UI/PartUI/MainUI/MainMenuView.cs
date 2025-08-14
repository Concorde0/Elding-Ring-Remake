using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

namespace RPG.UI
{
    public class MainMenuView : UIBaseView
    {
        protected override void BindEvents()
        {
            // 绑定按钮事件、文本等
            var vm = (MainMenuViewModel)ViewModel;
            // Example: button.onClick.AddListener(vm.OnStartClicked);
        }

        protected override void UnbindEvents()
        {
            
        }
    }
}

