using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework.Example;
using UnityEngine.EventSystems;

namespace QFramework
{
    public class UISlot : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
    {
        public Image Icon;
        public Text Count;
        
        public Slot Data { get; private set; }

        private bool mDragging = false;

        public UISlot InitWithData(Slot data)
        {
            Data = data;

            void UpdateView()
            {
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
            }
            Data.Changed.Register(UpdateView).
                UnRegisterWhenGameObjectDestroyed(gameObject);
            
            UpdateView();
            
            

            return this;
        }

        private void SyncItemToMousePos()
        {
            var mousePos = Input.mousePosition;
            if(RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePos, null,
                   out var localPos))
            {
                Icon.LocalPosition2D(localPos);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(mDragging || Data.Count == 0) return;
            mDragging = true;
            
            var canvas = Icon.gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000;
            
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
                mDragging = false;
                var canvas = Icon.GetComponent<Canvas>();
                canvas.DestroySelf();
                Icon.LocalPositionIdentity();
                
                if(ItemKit.CurrentSlotPointerOn)
                {
                    var uiSlot = ItemKit.CurrentSlotPointerOn;
                    var rectTransform = uiSlot.transform as RectTransform;
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
                    {
                        if (Data.Count != 0)
                        {
                            var cachedItem = uiSlot.Data.Item;
                            var cachedCount = uiSlot.Data.Count;

                            uiSlot.Data.Item = Data.Item;
                            uiSlot.Data.Count = Data.Count;

                            Data.Item = cachedItem;
                            Data.Count = cachedCount;
                            
                            uiSlot.Data.Changed.Trigger();
                            Data.Changed.Trigger();
                            
                        }
                        
                    }
                }
                else
                {
                    Data.Item = null;
                    Data.Count = 0;
                    
                    Data.Changed.Trigger();
                }
                
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemKit.CurrentSlotPointerOn = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (ItemKit.CurrentSlotPointerOn == this)
            {
                ItemKit.CurrentSlotPointerOn = null;
            }
        }
    }
}

