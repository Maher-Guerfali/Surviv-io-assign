using UnityEngine;

namespace Code.Infrastructure.AssetManagement
{
	public interface IAssetsService
	{
		GameObject LoadAssetFromResources(string path);
		TUnityObject LoadAssetFromResources<TUnityObject>(string path) where TUnityObject : UnityEngine.Object;
		TUnityObject[] LoadAssetsFromResources<TUnityObject>(string path) where TUnityObject : UnityEngine.Object;
	}
}