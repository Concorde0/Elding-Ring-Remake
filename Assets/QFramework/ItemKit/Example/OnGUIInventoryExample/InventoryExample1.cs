using System;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class InventoryExample1 : ViewController
	{
		
		
		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640,360);
			foreach (var slot in ItemKit.Slots)
			{
				GUILayout.BeginHorizontal("box");
				if (slot.Count == 0)
				{
					GUILayout.Label($"格子：空 ");
				}
				else
				{
					GUILayout.Label($"格子：{slot.Item.Name} 物品{slot.Item.Key} x {slot.Count}");
				}
				GUILayout.EndHorizontal();
				
			}
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品1 ");
			if (GUILayout.Button("+"))
			{
				if (!ItemKit.AddItem("Item_1"))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) {ItemKit. SubItem("Item_1"); }
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品2 ");
			if (GUILayout.Button("+"))
			{
				if (!ItemKit.AddItem("Item_2"))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) { ItemKit.SubItem("Item_2"); }
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品3 ");
			if (GUILayout.Button("+"))
			{
				if (!ItemKit.AddItem("Item_3"))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) { ItemKit.SubItem("Item_3"); }
			GUILayout.EndHorizontal();
			
		}

		

		

		
	}
}
