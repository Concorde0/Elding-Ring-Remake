using System;
using UnityEngine;
using QFramework;
using QFramework.Example;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class UGUIInventoryExample : ViewController
	{
		private void Awake()
		{
			ItemKit.LoadItemDatabase("ExampleItemDatabase");

			ItemKit.CreatSlotGroup("物品栏")
				.CreateSlot(ItemKit.ItemByKey[Items.item_Head], 1)
				.CreateSlot(ItemKit.ItemByKey[Items.item_BigSowrd],1)
				.CreateSlotByCount(8);
			
			ItemKit.CreatSlotGroup("背包")
				.CreateSlotByCount(20);

			ItemKit.CreatSlotGroup("武器")
				.CreateSlot(null,0);
		}

		void Start()
		{
			BtnAddItem1.onClick.AddListener(() =>
			{
				if (!ItemKit.GetSlotGroupByKey("物品栏").AddItem(Items.item_Head))
				{
					Debug.Log("背包已满");
				}
			});
			
			BtnAddItem2.onClick.AddListener(() =>
			{
				if (!ItemKit.GetSlotGroupByKey("物品栏").AddItem(Items.item_BigSowrd))
				{
					Debug.Log("背包已满");
				}
			});
			
			
			BtnDeleteItem1.onClick.AddListener(() =>
			{
				if (!ItemKit.GetSlotGroupByKey("物品栏").SubItem(Items.item_Head,10))
				{
					Debug.Log("数量不足");
				}
			});
			BtnDeleteItem2.onClick.AddListener(() =>
			{
				ItemKit.GetSlotGroupByKey("物品栏").SubItem(Items.item_BigSowrd);
			});
		}
		
	}
}
