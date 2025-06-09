using UnityEngine;

namespace Code.Infrastructure.UIManagement
{
	public interface IUIRootProvider
	{
		Transform UiRoot { get; }
		void SetUiRoot(Transform uiRoot);
	}
}