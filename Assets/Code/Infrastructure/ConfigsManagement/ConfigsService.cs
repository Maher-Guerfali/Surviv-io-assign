using System.Collections.Generic;
using System.Linq;
using Code.Gameplay.Characters.Enemies;
using Code.Gameplay.Characters.Enemies.Configs;
using Code.Gameplay.Characters.Heroes.Configs;
using Code.Gameplay.PickUps;
using Code.Gameplay.PickUps.Configs;
using Code.Infrastructure.AssetManagement;

namespace Code.Infrastructure.ConfigsManagement
{
	public class ConfigsService : IConfigsService
	{
		private readonly IAssetsService _assets;

		private Dictionary<EnemyId, EnemyConfig> _enemiesById = new();

		public DifficultyConfig DifficultyConfig { get; private set; }//New

		private Dictionary<PickUpId, PickUpConfig> _pickupsById = new();

		public HeroConfig HeroConfig { get; private set; }

		public ConfigsService(IAssetsService assets)
		{
			_assets = assets;
		}
		
		public void Load()
		{
			LoadHeroConfig();
			LoadEnemyConfigs();
			LoadPickUpConfigs();
			LoadDifficultyConfig();

		}

		private void LoadPickUpConfigs()
		{
			var pickUpConfigs = _assets.LoadAssetsFromResources<PickUpConfig>("Configs/PickUps");
			_pickupsById = pickUpConfigs.ToList().ToDictionary(x => x.Id, x => x);
		}

		private void LoadHeroConfig()
		{
			HeroConfig = _assets.LoadAssetFromResources<HeroConfig>("Configs/HeroConfig");
		}

		private void LoadEnemyConfigs()
		{
			var enemyConfigs = _assets.LoadAssetsFromResources<EnemyConfig>("Configs/Enemies");
			_enemiesById = enemyConfigs.ToList().ToDictionary(x => x.Id, x => x);
		}

		public EnemyConfig GetEnemyConfig(EnemyId id)
		{
			if (_enemiesById.TryGetValue(id, out EnemyConfig enemyConfig))
				return enemyConfig;

			throw new KeyNotFoundException($"Enemy config with id {id} not found");
		}
		
		public PickUpConfig GetPickUpConfig(PickUpId id)
		{
			if (_pickupsById.TryGetValue(id, out PickUpConfig pickUpConfig))
				return pickUpConfig;

			throw new KeyNotFoundException($"PickUp config with id {id} not found");
		}
		private void LoadDifficultyConfig()//new
		{
			DifficultyConfig = _assets.LoadAssetFromResources<DifficultyConfig>("Configs/DifficultyConfig");
		}

	}
}