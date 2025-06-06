using Code.Gameplay.PickUps.Behaviours;
using Code.Gameplay.PickUps.Configs;
using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.PickUps.Services
{
	public class PickUpFactory : IPickUpFactory
	{
		private readonly IInstantiateService _instantiateService;
		private readonly IConfigsService _configsService;

		public PickUpFactory(IInstantiateService instantiateService, IConfigsService configsService)
		{
			_instantiateService = instantiateService;
			_configsService = configsService;
		}
		
		public PickUp Create(PickUpId id, Vector3 at)
		{
			PickUpConfig pickUpConfig = _configsService.GetPickUpConfig(id);
			PickUp pickUp = _instantiateService.InstantiatePrefabForComponent(pickUpConfig.Prefab, at, Quaternion.identity);
			
			return pickUp;
		}
	}
}