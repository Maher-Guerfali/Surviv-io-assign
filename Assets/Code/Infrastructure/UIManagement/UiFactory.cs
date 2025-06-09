using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Infrastructure.UIManagement
{
	public class UiFactory : IUiFactory
	{
		private readonly IInstantiateService _instantiateService;
		private readonly IWindowInfoService _windowInfoService;
		private readonly IAssetsService _assetsService;

		private RuleTile.TilingRuleOutput.Transform _floatingTextsParent;

		public UiFactory(
			IInstantiateService instantiateService,
			IWindowInfoService windowInfoService, 
			IAssetsService assetsService)
		{
			_instantiateService = instantiateService;
			_windowInfoService = windowInfoService;
			_assetsService = assetsService;
		}
    
		public T CreateWindow<T>(Transform parent) where T : WindowBase
		{
			string address = _windowInfoService.GetWindowAddress<T>();
			var prefab = _assetsService.LoadAssetFromResources<T>(address);
      
			var instantiated = _instantiateService.InstantiatePrefabForComponent<T>(prefab, Vector3.zero, Quaternion.identity);
			((RectTransform)instantiated.transform).SetParent(parent, false);

			return instantiated;
		}
    
		public T CreateSubWindow<T>(string address, Transform parent) where T : SubWindowBase
		{
			var prefab = _assetsService.LoadAssetFromResources<T>(address);
			return _instantiateService.InstantiatePrefabForComponent<T>(prefab, Vector3.zero, Quaternion.identity, parent);
		}
	}
}