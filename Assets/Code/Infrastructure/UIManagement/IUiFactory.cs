using UnityEngine;

namespace Code.Infrastructure.UIManagement
{
	public interface IUiFactory
	{
		T CreateWindow<T>(Transform parent) where T : WindowBase;
		T CreateSubWindow<T>(string address, Transform parent) where T : SubWindowBase;
	}
}