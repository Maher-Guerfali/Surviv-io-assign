using Code.Gameplay.Abilities.Behaviours;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Services;
using Code.Gameplay.Cameras.Services;
using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.PickUps.Services;
using Code.Gameplay.Projectiles.Services;
using Code.Gameplay.UnitStats.Behaviours;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindHeroServices();
            BindEnemyServices();
            BindCameraServices();
            BindCombatServices();
            BindPickupServices();
            BindAbilityServices();
        }



        private void BindHeroServices()
        {
            Container.BindInterfacesTo<HeroFactory>().AsSingle();
            Container.BindInterfacesTo<HeroProvider>().AsSingle();
            Container.BindInterfacesTo<ExperienceService>().AsSingle(); // XP leveling

            // Bind LevelUpWindowBehaviour from the scene hierarchy
            Container.Bind<LevelUpWindowBehaviour>().FromComponentInHierarchy().AsSingle();

            // Bind Stats from the HeroProvider
            Container.Bind<Stats>().FromMethod(context =>
            {
                var heroProvider = context.Container.Resolve<IHeroProvider>();
                return heroProvider.Stats;
            }).AsSingle();
        }

        private void BindEnemyServices()
        {
            Container.BindInterfacesTo<EnemyFactory>().AsSingle();
            Container.BindInterfacesTo<EnemyProvider>().AsSingle();
            Container.BindInterfacesTo<EnemyDeathTracker>().AsSingle();
        }

        private void BindCameraServices()
        {
            Container.BindInterfacesTo<CameraProvider>().AsSingle();
        }

        private void BindCombatServices()
        {
            Container.BindInterfacesTo<ProjectileFactory>().AsSingle();
        }

        private void BindPickupServices()
        {
            Container.BindInterfacesTo<PickUpFactory>().AsSingle();
        }

        private void BindAbilityServices()
        {
            Container.Bind<IAbilityService>().To<AbilityService>().AsSingle();

            // Load from Resources/AbilityDatabase.asset
            var abilityDatabase = Resources.Load<AbilityDatabase>("Abilities/AbilityDatabase");

            if (abilityDatabase == null)
            {
                Debug.LogError("AbilityDatabase.asset not found in Resources folder!");
            }

            Container.Bind<AbilityDatabase>().FromInstance(abilityDatabase).AsSingle();
        }

    }
}
