using UnityEngine;
using QFramework;
using QFramework.Example;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class UGUIInventoryExample : ViewController
	{
		void Start()
		{
			ItemKit.LoadItemConfigByResources(nameof(Items.Head));
			ItemKit.LoadItemConfigByResources(nameof(Items.BigSword));
			
			ItemKit.CreateSlot(ItemKit.ItemByKey[Items.Head],1);
			ItemKit.CreateSlot(ItemKit.ItemByKey[Items.BigSword],1);
			
			UISlot.Hide();
			Refresh();
			

			BtnAddItem1.onClick.AddListener(() =>
			{
				if (!ItemKit.AddItem(Items.Head))
				{
					Debug.Log("背包已满");
				}
				Refresh();
			});
			
			BtnAddItem2.onClick.AddListener(() =>
			{
				if (!ItemKit.AddItem(Items.BigSword))
				{
					Debug.Log("背包已满");
				}
				Refresh();
			});
			
			
			BtnDeleteItem1.onClick.AddListener(() =>
			{
				if (!ItemKit.SubItem(Items.Head,10))
				{
					Debug.Log("数量不足");
				}
				Refresh();
			});
			BtnDeleteItem2.onClick.AddListener(() =>
			{
				ItemKit.SubItem(Items.BigSword);
				Refresh();
			});
		}

		public void Refresh()
		{
			UISlotRoot.DestroyChildren();
			foreach (var slot in ItemKit.Slots)
			{
				UISlot.InstantiateWithParent(UISlotRoot)
					.InitWithData(slot)
					.Show();
			}
		}
	}
}
