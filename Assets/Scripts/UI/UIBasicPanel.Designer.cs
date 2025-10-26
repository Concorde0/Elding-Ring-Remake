using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example1
{
	// Generate Id:5fc25266-e192-4238-97c5-cd6a84351d3e
	public partial class UIBasicPanel
	{
		public const string Name = "UIBasicPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public UnityEngine.UI.Button BtnAbout;
		[SerializeField]
		public UnityEngine.UI.Button BtnExit;
		
		private UIBasicPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnStart = null;
			BtnAbout = null;
			BtnExit = null;
			
			mData = null;
		}
		
		public UIBasicPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBasicPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBasicPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
