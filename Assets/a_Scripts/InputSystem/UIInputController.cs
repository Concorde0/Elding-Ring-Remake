using System.Collections;
using System.Collections.Generic;
using RPG.UI;
using UnityEngine;

namespace RPG.InputSystem
{
    public class UIInputController
    {
        private readonly PlayerInput _input;
        private readonly UIManager   _uiManager;

        public UIInputController(PlayerInput input, UIManager uiManager)
        {
            _input     = input;
            _uiManager = uiManager;

            // 绑定输入事件到 UI 管理器的窗口切换方法
            _input.GamePlay.Inventory.performed += _ => _uiManager.Toggle<InventoryWindowView, InventoryWindowViewModel>(StringConstants.WindowId.Inventory);

            _input.GamePlay.PlayerStats.performed += _ => _uiManager.Toggle<PlayerStatsWindowView, PlayerStatsWindowViewModel>(StringConstants.WindowId.PlayerStats);
        }

        public void Enable()  => _input.Enable();
        public void Disable() => _input.Disable();
    }
}

