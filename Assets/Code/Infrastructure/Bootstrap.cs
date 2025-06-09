using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.SceneManagement;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure
{
	public class Bootstrap : MonoBehaviour
	{
		private IConfigsService _configsService;
		private ISceneLoadService _sceneLoadService;

		[Inject]
		private void Construct(IConfigsService configsService, ISceneLoadService sceneLoadService)
		{
			_configsService = configsService;
			_sceneLoadService = sceneLoadService;
		}

		private void Start()
		{
			_configsService.Load();
			_sceneLoadService.LoadScene(Constants.BattleSceneName);
		}
	}
}