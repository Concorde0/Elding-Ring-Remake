using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public static class UIWindowRegistry
    {
        private static readonly Dictionary<string, string> _paths = new()
        {
            { "MainMenu", "UI/MainMenuWindow" },
            { "Settings", "UI/SettingsPanel" },
        };

        public static string GetPath(string windowId)
        {
            return _paths.TryGetValue(windowId, out var path) ? path : null;
        }
    }
}


