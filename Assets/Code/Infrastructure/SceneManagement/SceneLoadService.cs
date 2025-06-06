using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.SceneManagement
{
	public class SceneLoadService : ISceneLoadService
	{
		public async Task LoadScene(string name, Action onLoaded = null)
		{
			await Load(name, onLoaded);
		}

		private async Task Load(string nextScene, Action onLoaded = null)
		{
			if (SceneManager.GetActiveScene().name == nextScene)
			{
				onLoaded?.Invoke();
				return;
			}

			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
			
			if (asyncLoad == null)
			{
				throw new Exception($"Scene {nextScene} loading failed");
			}

			while (!asyncLoad.isDone)
			{
				await Task.Yield();
			}

			onLoaded?.Invoke();
		}
	}
}