using UnityEngine;

namespace Code.Infrastructure.UIManagement
{
	public class UiRootProvider : IUIRootProvider
	{ 
		public Transform UiRoot { get; private set; }
		
		public void SetUiRoot(Transform uiRoot)
		{
			UiRoot = uiRoot;
		}
	}
}