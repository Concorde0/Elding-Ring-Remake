// using UnityEngine;
//
// namespace RPG.UI
// {
//     public class SelectUIViewModel : UIBaseViewModel
//     {
//         public Vector2 ScreenPos { get; private set; }
//
//         // 如果需要，未来还可以传物品数据
//         // public IItemSlotData SlotData { get; private set; }
//
//         public void Init(Vector2 screenPos)
//         {
//             ScreenPos = screenPos;
//         }
//
//         public void OnUse()
//         {
//             Debug.Log($"[SelectVM] 使用物品，坐标 {ScreenPos}");
//            
//             CloseView();
//         }
//
//         public void OnDrop()
//         {
//             Debug.Log($"[SelectVM] 丢弃物品，坐标 {ScreenPos}");
//             
//             CloseView();
//         }
//
//         public void OnClose()
//         {
//             CloseView();
//         }
//
//         private void CloseView()
//         {
//             // if (View is MonoBehaviour mb)
//             //     Object.Destroy(mb.gameObject);
//         }
//     }
// }