using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Instantiation
{
	public interface IInstantiateService
	{
		void SetInstantiator(IInstantiator instantiator);
		T Instantiate<T>();
		T Instantiate<T>(IEnumerable<object> args);

		T Instantiate<T>(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent = null) 
			where T : Component;

		GameObject Instantiate(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent = null);
		T Instantiate<T>(GameObject prefab) where T : Component;

		T InstantiatePrefabForComponent<T>(T prefab, Vector3 at, Quaternion rotation, Transform parent = null) 
			where T : Component;
	}
}