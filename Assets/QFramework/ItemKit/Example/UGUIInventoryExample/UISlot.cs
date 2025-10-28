using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework.Example;
using UnityEngine.EventSystems;

namespace QFramework
{
    public class UISlot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        public Image Icon;
        public Text Count;
        
        public Slot Data { get; private set; }

        private bool mDragging = false;

        public UISlot InitWithData(Slot data)
        {
            Data = data;
            
            if (Data.Count == 0)
            {
                Icon.Hide();
                Count.text = "";
            }
            else
            {
                Icon.Show();
                if (data.Item.GetIcon)
                {
                    Icon.sprite = data.Item.GetIcon;
                }
                Count.text = Data.Count.ToString();
            }

            return this;
        }

        private void SyncItemToMousePos()
        {
            var mousePos = Input.mousePosition;
            var controller = FindAnyObjectByType<UGUIInventoryExample>();
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(controller.transform as RectTransform, mousePos, null,
                   out var localPos))
            {
                Icon.LocalPosition2D(localPos);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(mDragging || Data.Count == 0) return;
            mDragging = true;
            var controller = FindAnyObjectByType<UGUIInventoryExample>();
            Icon.Parent(controller);
            SyncItemToMousePos();

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (mDragging)
            {
                SyncItemToMousePos();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (mDragging)
            {
                Icon.Parent(transform);
                Icon.LocalPositionIdentity();

                var uiSlots = transform.parent.GetComponentsInChildren<UISlot>();

                bool throwItem = true;

                foreach (var uiSlot in uiSlots)
                {
                    var rectTransform = uiSlot.transform as RectTransform;
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
                    {
                        throwItem = false;
                        
                        if (Data.Count != 0)
                        {
                            var cachedItem = uiSlot.Data.Item;
                            var cachedCount = uiSlot.Data.Count;

                            uiSlot.Data.Item = Data.Item;
                            uiSlot.Data.Count = Data.Count;

                            Data.Item = cachedItem;
                            Data.Count = cachedCount;
                            
                            FindAnyObjectByType<UGUIInventoryExample>().Refresh();
                        }
                            
                        
                        break;
                    }
                }
                
                if (throwItem)
                {
                    Data.Item = null;
                    Data.Count = 0;
                    FindAnyObjectByType<UGUIInventoryExample>().Refresh();
                }
            }
        }
    }
}

