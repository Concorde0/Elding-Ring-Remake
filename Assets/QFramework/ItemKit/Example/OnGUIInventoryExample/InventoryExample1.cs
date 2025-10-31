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
		private void Awake()
		{
			ItemKit.CreatSlotGroup("背包2")
				.CreateSlotByCount(20);
		}
		

		private void Start()
		{
			ItemKit.LoadItemDatabase("ExampleItemDatabase");
			
			ItemKit.CreatSlotGroup("背包")
				.CreateSlot(ItemKit.ItemByKey[Items.item_Head],1)
				.CreateSlot(ItemKit.ItemByKey[Items.item_BigSowrd],1);
		}

		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640,360);
			foreach (var slot in ItemKit.GetSlotGroupByKey("背包").Slots)
			{
				GUILayout.BeginHorizontal("box");
				if (slot.Count == 0)
				{
					GUILayout.Label($"格子：空 ");
				}
				else
				{
					GUILayout.Label($"格子：{slot.Item.GetName} 物品{slot.Item.GetKey} x {slot.Count}");
				}
				GUILayout.EndHorizontal();
				
			}
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品1 ");
			if (GUILayout.Button("+"))
			{
				if (!ItemKit.GetSlotGroupByKey("背包").AddItem(Items.item_Head))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) {ItemKit.GetSlotGroupByKey("背包").SubItem(Items.item_Head); }
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品2 ");
			if (GUILayout.Button("+"))
			{
				if (!ItemKit.GetSlotGroupByKey("背包").AddItem(Items.item_BigSowrd))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) { ItemKit.GetSlotGroupByKey("背包").SubItem(Items.item_BigSowrd); }
			GUILayout.EndHorizontal();
			
			
		}

		

		

		
	}
}
