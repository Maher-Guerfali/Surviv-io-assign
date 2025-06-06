using Code.Gameplay.Cameras.Services;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.Instantiation;
using Code.Infrastructure.UIManagement;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
	public class BattleInitializer : MonoBehaviour, IInitializable
	{
		[SerializeField] private Camera _mainCamera;
		[SerializeField] private Transform _uiRoot;
		
		private ICameraProvider _cameraProvider;
		private IHeroFactory _heroFactory;
		private IInstantiator _instantiator;
		private IInstantiateService _instantiateService;
		private IUIRootProvider _uiRootProvider;
		private IUiService _uiService;
		private IConfigsService _configsService;//new
		[Inject]
		private void Construct(
			ICameraProvider cameraProvider,
			IHeroFactory heroFactory,
			IInstantiateService instantiateService,
			IInstantiator instantiator,
			IUIRootProvider uiRootProvider,
			IUiService uiService,
			IConfigsService configsService  // new
		)
		{
			_uiService = uiService;
			_uiRootProvider = uiRootProvider;
			_instantiateService = instantiateService;
			_instantiator = instantiator;
			_heroFactory = heroFactory;
			_cameraProvider = cameraProvider;
			_configsService = configsService;//new
		}
    
		public void Initialize()
		{
			_cameraProvider.SetMainCamera(_mainCamera);
			_instantiateService.SetInstantiator(_instantiator);
			_uiRootProvider.SetUiRoot(_uiRoot);
			
			_heroFactory.CreateHero(Vector3.zero, Quaternion.identity);

			_uiService.OpenWindow<HudWindow>();
		}
	}
}