using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
	public class SceneInitializationInstaller : MonoInstaller
	{
		[SerializeField] private List<MonoBehaviour> _initializers;
    
		public override void InstallBindings()
		{
			foreach (MonoBehaviour initializer in _initializers)
			{
				Container.BindInterfacesTo(initializer.GetType()).FromInstance(initializer).AsSingle();
			}
		}
	}
}