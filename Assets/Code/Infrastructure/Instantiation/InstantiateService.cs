using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Instantiation
{
	public class InstantiateService : IInstantiateService
    {
        private IInstantiator _instantiator;

        public InstantiateService(IInstantiator instantiator)
        {
            SetInstantiator(instantiator);
        }
        
        public void SetInstantiator(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public T Instantiate<T>()
        {
            return _instantiator.Instantiate<T>();
        }
        
        public T Instantiate<T>(IEnumerable<object> args)
        {
            return _instantiator.Instantiate<T>(args);
        }

        public T InstantiatePrefabForComponent<T>(T prefab, Vector3 at, Quaternion rotation, Transform parent = null) 
            where T : Component
        {
            var instantiated = _instantiator.InstantiatePrefabForComponent<T>(prefab, parent);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            Transform transform = instantiated.transform;
            transform.position = at;
            transform.rotation = rotation;
            
            return instantiated;
        }

        public T Instantiate<T>(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent = null) 
            where T : Component
        {
            var instantiated = _instantiator.InstantiatePrefabForComponent<T>(prefab, parent);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            Transform transform = instantiated.transform;
            transform.position = at;
            transform.rotation = rotation;
            
            return instantiated;
        }

        public GameObject Instantiate(GameObject prefab, Vector3 at, Quaternion rotation, Transform parent = null)
        {
            GameObject instantiated = _instantiator.InstantiatePrefab(prefab, parent);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            Transform transform = instantiated.transform;
            transform.position = at;
            transform.rotation = rotation;
            
            return instantiated;
        }

        public T Instantiate<T>(GameObject prefab) where T : Component
        {
            var instantiated = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            instantiated.name = instantiated.name.Replace("(Clone)", string.Empty);
            Transform transform = instantiated.transform;
            transform.rotation = Quaternion.identity;
            
            return instantiated;
        }
    }
}