using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class UISlotGroup: MonoBehaviour
    {
        public enum UISlotGenerateType
        {
            GenerateByTemplate,
            UseExistUISlot,
        }
        public string GroupKey;

        [DisplayLabel("UISlot 生成类型")] 
        public UISlotGenerateType Type;
        
        [DisplayIf(nameof(Type),false,UISlotGenerateType.GenerateByTemplate)]
        public UISlot UISlotTemplate;
        [DisplayIf(nameof(Type),false,UISlotGenerateType.GenerateByTemplate)]
        public RectTransform UISlotRoot;

        [DisplayIf(nameof(Type), false, UISlotGenerateType.UseExistUISlot)]
        public List<UISlot> UISlots;

        private void Start()
        {
            if (Type == UISlotGenerateType.GenerateByTemplate)
            {
                UISlotTemplate.Hide();
                Refresh();
            }
            else if (Type == UISlotGenerateType.UseExistUISlot)
            {
                for (int i = 0; i < UISlots.Count; i++)
                {
                   
                    UISlots[i].InitWithData(ItemKit.GetSlotGroupByKey(GroupKey).Slots[i]);
                }
                
            }
            
        }

        private void Update()
        {
           
          
        }

        private void Refresh()
        {
            UISlotRoot.DestroyChildrenWithCondition(child => child.GetComponent<UISlot>());
			foreach (var slot in ItemKit.GetSlotGroupByKey(GroupKey).Slots)
			{
				UISlotTemplate.InstantiateWithParent(UISlotRoot)
					.InitWithData(slot)
					.Show();
			}
        }
    }
}