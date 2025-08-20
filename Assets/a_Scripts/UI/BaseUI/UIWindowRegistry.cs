using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public static class UIWindowRegistry
    {
        private static readonly Dictionary<string, string> _paths = new()
        {
            { StringConstants.WindowId.MainWindow, "Prefab/UI/MainWindow" },
            { StringConstants.WindowId.Inventory, "Prefab/UI/InventoryWindow" }, 
            { StringConstants.WindowId.PlayerStats, "Prefab/UI/PlayerStatsWindow" },
        };

        public static string GetPath(string windowId)
        {
            return _paths.TryGetValue(windowId, out var path) ? path : null;
        }
    }
}


