using UnityEngine;

namespace Code.Infrastructure.AssetManagement
{
	public class AssetsService : IAssetsService
	{
		public GameObject LoadAssetFromResources(string path)
		{
			return Resources.Load<GameObject>(path);
		}

		public TUnityObject LoadAssetFromResources<TUnityObject>(string path) where TUnityObject : Object
		{
			return Resources.Load<TUnityObject>(path);
		}
		
		public TUnityObject[] LoadAssetsFromResources<TUnityObject>(string path) where TUnityObject : Object
		{
			return Resources.LoadAll<TUnityObject>(path);
		}
	}
}