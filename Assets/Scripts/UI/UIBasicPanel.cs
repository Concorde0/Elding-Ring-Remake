using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example1
{
	public class UIBasicPanelData : UIPanelData
	{
		public int count;
	}
	public partial class UIBasicPanel : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBasicPanelData ?? new UIBasicPanelData();
			// please add init code here

			UIKit.OpenPanel<UIBasicPanel>(new UIBasicPanelData()
			{
				count = 10
			});
			BtnStart.onClick.AddListener(() =>
			{
				Debug.Log("Start Button Clicked! Count: " + mData.count);
			});
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
