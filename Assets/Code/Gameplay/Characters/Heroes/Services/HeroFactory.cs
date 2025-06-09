using Code.Gameplay.Characters.Heroes.Configs;
using Code.Gameplay.Guns.Behaviours;
using Code.Gameplay.Identification.Behaviours;
using Code.Gameplay.Lifetime.Behaviours;
using Code.Gameplay.UnitStats;
using Code.Gameplay.UnitStats.Behaviours;
using Code.Infrastructure.ConfigsManagement;
using Code.Infrastructure.Identification;
using Code.Infrastructure.Instantiation;
using UnityEngine;

namespace Code.Gameplay.Characters.Heroes.Services
{
    public class HeroFactory : IHeroFactory
    {
        private readonly IConfigsService _configsService;
        private readonly IInstantiateService _instantiateService;
        private readonly IIdentifierService _identifiers;
        private readonly IHeroProvider _heroProvider;

        public HeroFactory(
            IConfigsService configsService,
            IHeroProvider heroProvider,
            IInstantiateService instantiateService,
            IIdentifierService identifiers)
        {
            _configsService = configsService;
            _heroProvider = heroProvider;
            _instantiateService = instantiateService;
            _identifiers = identifiers;
        }

        public Behaviours.Hero CreateHero(Vector3 at, Quaternion rotation)
        {
            HeroConfig heroConfig = _configsService.HeroConfig;
            Behaviours.Hero hero = _instantiateService.InstantiatePrefabForComponent(heroConfig.Prefab, at, rotation);

            hero.GetComponent<Id>()
                .Setup(_identifiers.Next());

            hero.GetComponent<Stats>()
                // Base stats
                .SetBaseStat(StatType.MaxHealth, heroConfig.Health)
                .SetBaseStat(StatType.MovementSpeed, heroConfig.MovementSpeed)
                .SetBaseStat(StatType.ProjectileSpeed, heroConfig.ProjectileSpeed)
                .SetBaseStat(StatType.VisionRange, heroConfig.VisionRange)
                .SetBaseStat(StatType.ShootCooldown, heroConfig.ShootCooldown)
                .SetBaseStat(StatType.Damage, heroConfig.Damage)
                // XP stats
                .SetBaseStat(StatType.CurrentXP, heroConfig.StartingXP)
                .SetBaseStat(StatType.RequiredXP, heroConfig.XPRequiredPerLevel)
                .SetBaseStat(StatType.Level, heroConfig.StartingLevel)
                // Initialize ability stats to 0
                .SetBaseStat(StatType.HealthPotionEffectiveness, 0f)
                .SetBaseStat(StatType.Piercing, 0f)
                .SetBaseStat(StatType.Bounces, 0f)
                .SetBaseStat(StatType.OrbitingProjectiles, 0f);

            hero.GetComponent<Health>()
                .Setup(heroConfig.Health, heroConfig.Health);

            hero.GetComponent<GunOwner>().OwnedGun
                .GetComponent<Stats>()
                .SetBaseStat(StatType.RotationSpeed, heroConfig.GunRotationSpeed);

            _heroProvider.SetHero(hero);

            return hero;
        }
    }
}