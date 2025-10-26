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

		
		void Start()
		{
			// Code Here
		}
		
		public class Slot
		{
			public Item Item;
			public int Count;

			public Slot(Item item, int count)
			{
				Item = item;
				Count = count;
			}
		}

		public class Item
		{
			public string Key;
			public string Name;

			public Item(string key, string name)
			{
				Key = key;
				name = name;
			}
		}
		
		public Item Item1 = new Item("Item_1", "物品1");
		public Item Item2 = new Item("Item_2", "物品2");
		public Item Item3 = new Item("Item_3", "物品3");
		
		private List<Slot> mSlots = null;
		private Dictionary<string, Item> mItemByKey = null;

		private void Awake()
		{
			mSlots = new List<Slot>()
			{
				new Slot(Item1,1),
				new Slot(Item2,10),
				new Slot(Item3,1),
				
			};

			mItemByKey = new Dictionary<string, Item>()
			{
			
				{Item1.Key,Item1},
				{Item2.Key,Item2},
				{Item3.Key,Item3},
			
			};
		}
		
		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640,360);
			foreach (var slot in mSlots)
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
				if (!AddItem("Item_1"))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) { SubItem("Item_1"); }
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品2 ");
			if (GUILayout.Button("+"))
			{
				if (!AddItem("Item_2"))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) { SubItem("Item_2"); }
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label($"物品3 ");
			if (GUILayout.Button("+"))
			{
				if (!AddItem("Item_3"))
				{
					Debug.Log("物品栏已满");
				}
			}
			if (GUILayout.Button("-")) { SubItem("Item_3"); }
			GUILayout.EndHorizontal();
			
		}

		private Slot FindSlotByKey(string itemKey)
		{
			return mSlots.Find(s => s.Item != null && s.Item.Key == itemKey && s.Count != 0);
		}

		private Slot FindEmptySlot()
		{
			return	mSlots.Find(s => s.Count == 0);
		}

		private Slot FindAddableSlot(string itemKey)
		{
			var slot = FindSlotByKey(itemKey);
			if (slot == null)
			{
				slot = FindEmptySlot();
				if (slot != null)
				{
					slot.Item = mItemByKey[itemKey];
				}
			}

			return slot;
		}

		private bool AddItem(string itemKey, int addCount = 1)
		{
			var slot = FindAddableSlot(itemKey);
			if (slot == null)
			{
				return false;
			}
			slot.Count += addCount;
			return true;
		}

		bool SubItem(string itemKey, int subCount = 1)
		{
			var slot = FindSlotByKey(itemKey);
			if (slot != null)
			{
				slot.Count -= subCount;
				return true;
			}

			return false;
		}
	}
}
