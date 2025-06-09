using Code.Gameplay.Abilities.Behaviours;
using Code.Gameplay.Abilities.Configs;
using Code.Gameplay.Abilities.Services;
using Code.Gameplay.Cameras.Services;
using Code.Gameplay.Characters.Enemies.Services;
using Code.Gameplay.Characters.Heroes.Services;
using Code.Gameplay.PickUps.Services;
using Code.Gameplay.Projectiles.Services;
using Code.Gameplay.UnitStats.Behaviours;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BattleInstaller : MonoInstaller
    {
        [Header("UI References")]
        [SerializeField] private HudWindow hudWindow;
        [SerializeField] private LevelUpWindowBehaviour levelUpWindow;

        public override void InstallBindings()
        {
            BindHeroServices();
            BindEnemyServices();
            BindCameraServices();
            BindCombatServices();
            BindPickupServices();
            BindAbilityServices();
            BindUIServices();
        }

        private void BindHeroServices()
        {
            // Bind interfaces properly
            Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
            Container.Bind<IHeroProvider>().To<HeroProvider>().AsSingle();
            Container.BindInterfacesTo<ExperienceService>().AsSingle(); // XP leveling
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
                Debug.LogError("AbilityDatabase.asset not found in Resources/Abilities/ folder!");
            }
            Container.Bind<AbilityDatabase>().FromInstance(abilityDatabase).AsSingle();
        }

        private void BindUIServices()
        {
            // Bind UI components from serialized references instead of hierarchy search
            if (hudWindow != null)
            {
                Container.Bind<HudWindow>().FromInstance(hudWindow).AsSingle();
            }
            else
            {
                Debug.LogWarning("HudWindow reference not set in BattleInstaller!");
                // Fallback to hierarchy search if needed
                Container.Bind<HudWindow>().FromComponentInHierarchy().AsSingle();
            }

            if (levelUpWindow != null)
            {
                Container.Bind<LevelUpWindowBehaviour>().FromInstance(levelUpWindow).AsSingle();
            }
            else
            {
                Debug.LogWarning("LevelUpWindowBehaviour reference not set in BattleInstaller!");
                // Fallback to hierarchy search if needed
                Container.Bind<LevelUpWindowBehaviour>().FromComponentInHierarchy().AsSingle();
            }
        }

        // Validation method to check if references are properly set
        private void OnValidate()
        {
            if (hudWindow == null)
                Debug.LogWarning("HudWindow reference is not set in BattleInstaller!");

            if (levelUpWindow == null)
                Debug.LogWarning("LevelUpWindowBehaviour reference is not set in BattleInstaller!");
        }
    }
}