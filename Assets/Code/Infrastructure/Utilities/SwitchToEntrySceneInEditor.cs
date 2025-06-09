using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Utilities
{
	/// <summary>
	/// Should have such execution order, so it starts before every other script.
	/// </summary>
	public class SwitchToEntrySceneInEditor : MonoBehaviour
	{
		#if UNITY_EDITOR
		private const int EntrySceneIndex = 0;

		private void Awake()
		{
			if (ProjectContext.HasInstance) 
				return;
      
			foreach (GameObject root in gameObject.scene.GetRootGameObjects()) 
				root.SetActive(false);
      
			SceneManager.LoadScene(EntrySceneIndex);
		}
		#endif
	}
}